namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.情报板1默认设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.可变信息板默认设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.交通诱导灯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.投影灯默认设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上海勋飞微波车检器设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.天津五维地磁车检器设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nettype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netstate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.keptimer1 = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.菜单ToolStripMenuItem.Text = "菜单";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加设备ToolStripMenuItem,
            this.情报板1默认设置ToolStripMenuItem,
            this.可变信息板默认设置ToolStripMenuItem,
            this.交通诱导灯ToolStripMenuItem,
            this.投影灯默认设置ToolStripMenuItem,
            this.上海勋飞微波车检器设置ToolStripMenuItem,
            this.天津五维地磁车检器设置ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 添加设备ToolStripMenuItem
            // 
            this.添加设备ToolStripMenuItem.Name = "添加设备ToolStripMenuItem";
            this.添加设备ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.添加设备ToolStripMenuItem.Text = "添加设备";
            this.添加设备ToolStripMenuItem.Click += new System.EventHandler(this.添加设备ToolStripMenuItem_Click);
            // 
            // 情报板1默认设置ToolStripMenuItem
            // 
            this.情报板1默认设置ToolStripMenuItem.Name = "情报板1默认设置ToolStripMenuItem";
            this.情报板1默认设置ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.情报板1默认设置ToolStripMenuItem.Text = "情报板1默认设置";
            this.情报板1默认设置ToolStripMenuItem.Click += new System.EventHandler(this.情报板1默认设置ToolStripMenuItem_Click);
            // 
            // 可变信息板默认设置ToolStripMenuItem
            // 
            this.可变信息板默认设置ToolStripMenuItem.Name = "可变信息板默认设置ToolStripMenuItem";
            this.可变信息板默认设置ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.可变信息板默认设置ToolStripMenuItem.Text = "可变信息板默认设置";
            this.可变信息板默认设置ToolStripMenuItem.Click += new System.EventHandler(this.可变信息板默认设置ToolStripMenuItem_Click);
            // 
            // 交通诱导灯ToolStripMenuItem
            // 
            this.交通诱导灯ToolStripMenuItem.Name = "交通诱导灯ToolStripMenuItem";
            this.交通诱导灯ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.交通诱导灯ToolStripMenuItem.Text = "交通诱导灯默认设置";
            this.交通诱导灯ToolStripMenuItem.Click += new System.EventHandler(this.交通诱导灯ToolStripMenuItem_Click);
            // 
            // 投影灯默认设置ToolStripMenuItem
            // 
            this.投影灯默认设置ToolStripMenuItem.Name = "投影灯默认设置ToolStripMenuItem";
            this.投影灯默认设置ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.投影灯默认设置ToolStripMenuItem.Text = "投影灯默认设置";
            this.投影灯默认设置ToolStripMenuItem.Click += new System.EventHandler(this.投影灯默认设置ToolStripMenuItem_Click);
            // 
            // 上海勋飞微波车检器设置ToolStripMenuItem
            // 
            this.上海勋飞微波车检器设置ToolStripMenuItem.Name = "上海勋飞微波车检器设置ToolStripMenuItem";
            this.上海勋飞微波车检器设置ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.上海勋飞微波车检器设置ToolStripMenuItem.Text = "上海勋飞微波车检器设置";
            this.上海勋飞微波车检器设置ToolStripMenuItem.Click += new System.EventHandler(this.上海勋飞微波车检器设置ToolStripMenuItem_Click);
            // 
            // 天津五维地磁车检器设置ToolStripMenuItem
            // 
            this.天津五维地磁车检器设置ToolStripMenuItem.Name = "天津五维地磁车检器设置ToolStripMenuItem";
            this.天津五维地磁车检器设置ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.天津五维地磁车检器设置ToolStripMenuItem.Text = "天津五维地磁车检器设置";
            this.天津五维地磁车检器设置ToolStripMenuItem.Click += new System.EventHandler(this.天津五维地磁车检器设置ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.Column2,
            this.Column3,
            this.id,
            this.Column4,
            this.Column5,
            this.addr,
            this.IP,
            this.nettype,
            this.myport,
            this.netstate,
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.Size = new System.Drawing.Size(1008, 424);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "device_type";
            this.name.HeaderText = "设备类型";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.DataPropertyName = "kepsever_ip";
            this.Column2.HeaderText = "服务器地址";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.DataPropertyName = "kepsever_port";
            this.Column3.HeaderText = "服务器端口";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.id.DataPropertyName = "id10";
            this.id.HeaderText = "从站地址";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.DataPropertyName = "kepsever_state";
            this.Column4.HeaderText = "服务器状态";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.DataPropertyName = "kepsever_log";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "服务器日志";
            this.Column5.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column5.HeaderText = "服务器日志";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // addr
            // 
            this.addr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.addr.DataPropertyName = "addr10";
            this.addr.HeaderText = "设备编号";
            this.addr.Name = "addr";
            this.addr.ReadOnly = true;
            // 
            // IP
            // 
            this.IP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IP.DataPropertyName = "IP";
            this.IP.HeaderText = "设备IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            // 
            // nettype
            // 
            this.nettype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nettype.DataPropertyName = "nettype";
            this.nettype.HeaderText = "网络类型";
            this.nettype.Name = "nettype";
            this.nettype.ReadOnly = true;
            // 
            // myport
            // 
            this.myport.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.myport.DataPropertyName = "myport";
            this.myport.HeaderText = "设备端口";
            this.myport.Name = "myport";
            this.myport.ReadOnly = true;
            // 
            // netstate
            // 
            this.netstate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.netstate.DataPropertyName = "netstate";
            this.netstate.HeaderText = "网络状态";
            this.netstate.Name = "netstate";
            this.netstate.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "监听日志";
            this.Column1.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column1.HeaderText = "监听日志";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // keptimer1
            // 
            this.keptimer1.Tick += new System.EventHandler(this.keptimer1_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 5000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "调度中心";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 情报板1默认设置ToolStripMenuItem;
        public System.Windows.Forms.Timer keptimer1;
        public System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 可变信息板默认设置ToolStripMenuItem;
        public System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewButtonColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn addr;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn nettype;
        private System.Windows.Forms.DataGridViewTextBoxColumn myport;
        private System.Windows.Forms.DataGridViewTextBoxColumn netstate;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.ToolStripMenuItem 交通诱导灯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 投影灯默认设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上海勋飞微波车检器设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 天津五维地磁车检器设置ToolStripMenuItem;
    }
}

