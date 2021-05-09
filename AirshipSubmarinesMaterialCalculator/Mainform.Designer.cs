namespace Kuranado.Moe.FFXIV
{
    partial class Mainform
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
            this.tabMainLayout = new System.Windows.Forms.TabControl();
            this.tpCraftCalc = new System.Windows.Forms.TabPage();
            this.scShowLayout = new System.Windows.Forms.SplitContainer();
            this.tlpPartsMenu = new System.Windows.Forms.TableLayoutPanel();
            this.lvMaterialDetails = new System.Windows.Forms.ListView();
            this.partColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.normalMaterialColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.npcMaterialColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpSuitDesign = new System.Windows.Forms.TabPage();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.tabMainLayout.SuspendLayout();
            this.tpCraftCalc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scShowLayout)).BeginInit();
            this.scShowLayout.Panel1.SuspendLayout();
            this.scShowLayout.Panel2.SuspendLayout();
            this.scShowLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMainLayout
            // 
            this.tabMainLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMainLayout.Controls.Add(this.tpCraftCalc);
            this.tabMainLayout.Controls.Add(this.tpSuitDesign);
            this.tabMainLayout.Location = new System.Drawing.Point(12, 12);
            this.tabMainLayout.Name = "tabMainLayout";
            this.tabMainLayout.SelectedIndex = 0;
            this.tabMainLayout.Size = new System.Drawing.Size(728, 705);
            this.tabMainLayout.TabIndex = 0;
            // 
            // tpCraftCalc
            // 
            this.tpCraftCalc.AutoScroll = true;
            this.tpCraftCalc.Controls.Add(this.scShowLayout);
            this.tpCraftCalc.Location = new System.Drawing.Point(4, 22);
            this.tpCraftCalc.Name = "tpCraftCalc";
            this.tpCraftCalc.Padding = new System.Windows.Forms.Padding(3);
            this.tpCraftCalc.Size = new System.Drawing.Size(720, 679);
            this.tpCraftCalc.TabIndex = 0;
            this.tpCraftCalc.Text = "素材计算";
            this.tpCraftCalc.UseVisualStyleBackColor = true;
            // 
            // scShowLayout
            // 
            this.scShowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scShowLayout.Location = new System.Drawing.Point(3, 3);
            this.scShowLayout.Name = "scShowLayout";
            // 
            // scShowLayout.Panel1
            // 
            this.scShowLayout.Panel1.Controls.Add(this.tlpPartsMenu);
            // 
            // scShowLayout.Panel2
            // 
            this.scShowLayout.Panel2.Controls.Add(this.lvMaterialDetails);
            this.scShowLayout.Size = new System.Drawing.Size(714, 673);
            this.scShowLayout.SplitterDistance = 324;
            this.scShowLayout.TabIndex = 1;
            // 
            // tlpPartsMenu
            // 
            this.tlpPartsMenu.ColumnCount = 5;
            this.tlpPartsMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpPartsMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpPartsMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpPartsMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpPartsMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpPartsMenu.Location = new System.Drawing.Point(3, 3);
            this.tlpPartsMenu.Name = "tlpPartsMenu";
            this.tlpPartsMenu.RowCount = 11;
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tlpPartsMenu.Size = new System.Drawing.Size(318, 667);
            this.tlpPartsMenu.TabIndex = 0;
            // 
            // lvMaterialDetails
            // 
            this.lvMaterialDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.partColumn,
            this.normalMaterialColumn,
            this.npcMaterialColumn});
            this.lvMaterialDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMaterialDetails.HideSelection = false;
            this.lvMaterialDetails.Location = new System.Drawing.Point(0, 0);
            this.lvMaterialDetails.Name = "lvMaterialDetails";
            this.lvMaterialDetails.Size = new System.Drawing.Size(386, 673);
            this.lvMaterialDetails.TabIndex = 0;
            this.lvMaterialDetails.UseCompatibleStateImageBehavior = false;
            this.lvMaterialDetails.View = System.Windows.Forms.View.Details;
            // 
            // partColumn
            // 
            this.partColumn.Text = "部件列表";
            this.partColumn.Width = 62;
            // 
            // normalMaterialColumn
            // 
            this.normalMaterialColumn.Text = "普通素材";
            this.normalMaterialColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.normalMaterialColumn.Width = 184;
            // 
            // npcMaterialColumn
            // 
            this.npcMaterialColumn.Text = "可兑换/购买素材";
            this.npcMaterialColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.npcMaterialColumn.Width = 139;
            // 
            // tpSuitDesign
            // 
            this.tpSuitDesign.Location = new System.Drawing.Point(4, 22);
            this.tpSuitDesign.Name = "tpSuitDesign";
            this.tpSuitDesign.Padding = new System.Windows.Forms.Padding(3);
            this.tpSuitDesign.Size = new System.Drawing.Size(720, 679);
            this.tpSuitDesign.TabIndex = 1;
            this.tpSuitDesign.Text = "配装设计";
            this.tpSuitDesign.UseVisualStyleBackColor = true;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitch.Location = new System.Drawing.Point(652, 5);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(88, 23);
            this.btnSwitch.TabIndex = 1;
            this.btnSwitch.Text = "切换到潜水艇";
            this.btnSwitch.UseVisualStyleBackColor = true;
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 729);
            this.Controls.Add(this.btnSwitch);
            this.Controls.Add(this.tabMainLayout);
            this.Name = "Mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[kuranado]飞空艇潜水艇配装计算器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabMainLayout.ResumeLayout(false);
            this.tpCraftCalc.ResumeLayout(false);
            this.scShowLayout.Panel1.ResumeLayout(false);
            this.scShowLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scShowLayout)).EndInit();
            this.scShowLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMainLayout;
        private System.Windows.Forms.TabPage tpCraftCalc;
        private System.Windows.Forms.TabPage tpSuitDesign;
        private System.Windows.Forms.SplitContainer scShowLayout;
        private System.Windows.Forms.TableLayoutPanel tlpPartsMenu;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.ListView lvMaterialDetails;
        private System.Windows.Forms.ColumnHeader partColumn;
        private System.Windows.Forms.ColumnHeader normalMaterialColumn;
        private System.Windows.Forms.ColumnHeader npcMaterialColumn;
    }
}

