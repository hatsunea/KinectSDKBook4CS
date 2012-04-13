using System.Windows.Forms ;
using System.Drawing ;
using System.Collections.Generic;
using System.IO;
using System;

namespace Hisui.Gui
{
  public class MainForm2 : Form
  {
    public readonly ViewPanel ViewPanel = CreateViewPanel();
    public readonly TreePanel2 TreePanel = CreateTreePanel();
    public readonly MenuStrip MainMenu;
    public readonly Form VersionDialog;
    public readonly SplitContainer SplitContainer;
    public readonly ToolStripContainer ToolStripContainer = new ToolStripContainer();
    public readonly StatusBar StatusBar = new StatusBar();

    public MainForm2( string title, Form versionDlg )
    {
      // �v���O���X�o�[�̃_�C�A���O�\��
      new ProgressDialog().SetUp( this, false );

      this.VersionDialog = versionDlg;
      this.MainMenu = CreateMainMenu( versionDlg );
      this.SplitContainer = CreateSplitContainer( this.ViewPanel, this.TreePanel );

      // �c�[���o�[�̍쐬
      foreach ( ToolStrip ts in Ctrl.ToolStripRegistry.ToolStrips ) {
        ToolStripContainer.TopToolStripPanel.Controls.Add( ts );
      }

      // _container
      ToolStripContainer.Dock = DockStyle.Fill;
      ToolStripContainer.TopToolStripPanelVisible = true;
      ToolStripContainer.TopToolStripPanel.Controls.Add( this.MainMenu );
      ToolStripContainer.BottomToolStripPanelVisible = true;
      ToolStripContainer.BottomToolStripPanel.Controls.Add( StatusBar );
      ToolStripContainer.ContentPanel.Controls.Add( SplitContainer );

      // this
      this.Controls.Add( ToolStripContainer );

      // ������
      SI.Tasks.Add( this.ViewPanel );
      SI.Tasks.Add( this.TreePanel );
      SI.PushIdlingSelection( SI.DocumentViews, true );  // �G���g���̑I��
      SI.SetupMainForm( this, title );

      // �ݒ�t�@�C���̓ǂݏ���
      SetupFormSettings( this );
    }

    public MainForm2( string title )
      : this( title, new AboutBox() )
    { }

    public MainForm2()
      : this( "�q�X�C [������ЃJ�^�b�`]" )
    { }


    /// <summary>
    /// <see cref="SingleViewPanel"/> �C���X�^���X�𐶐����A<c>SetUp()</c> �֐���������
    /// <see cref="SI.DocumentViews"/> ���w�肵�ČĂяo���ď��������A�Ԃ��܂��B
    /// </summary>
    /// <returns>�������ς݂� <see cref="SingleViewPanel"/> �C���X�^���X</returns>
    public static ViewPanel CreateViewPanel()
    {
      //ViewPanel view = new QuadViewPanel() { Dock = DockStyle.Fill };
      ViewPanel view = new SingleViewPanel() { Dock = DockStyle.Fill };
      view.SetUp( SI.DocumentViews );
      return view;
    }


    /// <summary>
    /// <see cref="TreePanel2"/> �C���X�^���X�𐶐����A<see cref="TreePanel2.SetUp"/> �֐���������
    /// <see cref="SI.DocumentViews"/> ���w�肵�ČĂяo���ď��������A�Ԃ��܂��B
    /// </summary>
    /// <returns>�������ς݂� <see cref="TreePanel2"/> �C���X�^���X</returns>
    public static TreePanel2 CreateTreePanel()
    {
      var tree = new TreePanel2 { Dock = DockStyle.Fill };
      tree.SetUp( SI.DocumentViews );
      return tree;
    }


    public static MenuStrip CreateMainMenu( Form versionDlg )
    {
      var help = new ToolStripMenuItem( "�w���v" );
      var version = new ToolStripMenuItem( "�o�[�W�������" );
      help.DropDownItems.Add( version );
      version.Click += ( sender, e ) => versionDlg.ShowDialog();
      MenuStrip menu = SI.CreateMainMenu();
      menu.Items.Add( help );
      return menu;
    }


    public static SplitContainer CreateSplitContainer( Control viewPanel, Control treePanel )
    {
      SplitContainer split = new SplitContainer();
      split.Dock = DockStyle.Fill;
      split.SplitterDistance = 7 * split.Width / 10;
      split.Panel1.Controls.Add( viewPanel );
      split.Panel2.Controls.Add( treePanel );
      return split;
    }


    /// <summary>
    /// �t�H�[���̈ʒu��傫���̐ݒ�t�@�C��I/O��ݒ肵�܂��B
    /// �ݒ�t�@�C����ǂݍ���Œl���t�H�[���ɐݒ肵�A<see cref="Form.FormClosing"/> �C�x���g�Őݒ�l���������܂��悤�ɂ��܂��B
    /// </summary>
    /// <param name="form"></param>
    public static void SetupFormSettings( Form form )
    {
      MainForm.SetupFormSettings( form );
    }
  }
}
