using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading;
using Liteon.Mes.Utility;

namespace WindowFramework
{
    public partial class frmMain : Form
    {
        UserInfo userInfo = null;
        BLL bll = new BLL();
        Dictionary<string, IMesForm> plugins = null;
        //string uid = "90003652";
      
        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            userInfo = bll.GetCurrentUserInfo(Para.UID);
            if (userInfo == null)
            {
                MessageBox.Show("查詢不到當前用戶的信息，請先維護", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            this.Text = Para.PROGRAM;
            this.ssp1_status.Text = "正在加载窗体菜单项...";
            Application.DoEvents();
            Thread.Sleep(200);
            try
            {
                SetMenuStatus(Para.UID, mnu);
                //ToolStripMenuItem mWin = new ToolStripMenuItem("Window");
                //mnu.Items.Add(mWin);
                ToolStripMenuItem mExit = new ToolStripMenuItem("Exit");
                mExit.Click += new System.EventHandler(ExitApp);
                mnu.Items.Add(mExit);
                //mnu.MdiWindowListItem = mWin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.ssp1_status.Text = "當前用戶：" + userInfo.Emp_No 
                + "（"+userInfo.Emp_Name+"）， 所屬部門：" + userInfo.Dept
                +"    [ 主程序版本："+Para.VERSION+" ]";
        }

        private void ExitApp(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("要退出程序嗎？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Application.Exit();
                return;
            }
        }

        /// <summary>
        /// 打開指定的窗體，如果已經實例化，則直接打開，否則先創建
        /// </summary>
        /// <param name="frm"></param>
        private void ShowWindow(System.Windows.Forms.Form frm)
        {
            foreach (Form mdiForm in this.MdiChildren)
            {
                if (mdiForm.GetType().Equals(frm.GetType()))
                {
                    mdiForm.Activate();
                    frm.Dispose();
                    return;
                }
            }
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        /// <summary>
        /// 按權限顯示菜單項目
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menu"></param>
        private void SetMenuStatus(string userId, MenuStrip menu)
        {
            using (DataTable dtMenu = bll.GetUserMenu(Para.UID))
            {
                //先顯示主菜單
                var topMenu = (from m in dtMenu.AsEnumerable() select m["MENU"]).Distinct();
                foreach (var one in topMenu)
                {
                    ToolStripMenuItem mi = new ToolStripMenuItem(one.ToString());
                    menu.Items.Add(mi);
                    //再顯示主菜單下的子菜單
                    var menuDetail = (from dl in dtMenu.AsEnumerable() where dl.Field<string>("MENU") == one.ToString() select dl["FUNCTION"]+"/"+dl["FUNCTION_ID"]);
                    foreach(var detail in  menuDetail)
                    {
                        string[] array = detail.ToString().Split('/');
                        ToolStripMenuItem sub = new ToolStripMenuItem(array[0]);
                        sub.Tag = array[1];
                        mi.DropDownItems.Add(sub);
                        sub.Click += new EventHandler(MenuClick);
                    }
                }
            }
        }

        /// <summary>
        /// 菜單單擊事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick(object sender, EventArgs e)
        {
            //ToolStripItem menuForm = (ToolStripItem)sender;
            //ToolStripItem menuProg = menuForm.OwnerItem;
            //ToolStripItem menuDept = menuProg.OwnerItem;

            Liteon.Mes.Db.Oracle.CreateConnectionStringByXMLFile("DBConnection.xml");//有可能界面連接了其他DB，會導致查詢menu的時候不是連接打開程序時候的DB，所以每次點按鈕都重新讀取一下
            ToolStripItem menuClicked = (ToolStripItem)sender;

            using (DataTable dt = bll.GetFunctionInfo(menuClicked.Tag.ToString()))
            {
                if (dt == null)
                {
                    //BLL.Tool.MsgBox(2, "没有获取窗口设置信息");
                    return;
                }
                else
                {
                    try
                    {
                        //給窗體傳送啟動參數
                        string[] startArgs = new string[7];
                        startArgs[0] = Para.UID;
                        startArgs[1] = dt.Rows[0]["HOST_NAME"].ToString();
                        startArgs[2] = Liteon.Mes.Utility.Encrypt.DesDecrypt(dt.Rows[0]["ACCOUNT"].ToString());
                        startArgs[3] = Liteon.Mes.Utility.Encrypt.DesDecrypt(dt.Rows[0]["PASSWORD"].ToString());
                        startArgs[4] = dt.Rows[0]["SERVICE_NAME"].ToString();
                        startArgs[5] = Para.PROGRAM;
                        startArgs[6] = "";
                        List<MesForm> plugin = FormLoader.LoadOneForm(Path.Combine(Application.StartupPath, dt.Rows[0]["DLL_NAME"].ToString()));
                        Form frm = plugin[0].CreateForm(dt.Rows[0]["ASSEMBLY_NAME"].ToString(),dt.Rows[0]["NAMESPACE"].ToString(), dt.Rows[0]["FORM_NAME"].ToString(), startArgs);
                        if (frm == null)
                        {
                            MessageBox.Show("實例化窗口失敗，請檢查命名空間（程序集）和窗口名稱設置", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        ShowWindow(frm);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("實例化窗口失敗\r\n"+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


 





    }
}
