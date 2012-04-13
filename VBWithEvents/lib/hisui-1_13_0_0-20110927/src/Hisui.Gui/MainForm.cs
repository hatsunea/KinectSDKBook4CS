using System.Windows.Forms ;
using System.Drawing ;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Hisui.Gui
{
  public class MainForm : Form
  {
    public readonly ViewPanel ViewPanel = CreateViewPanel();
    public readonly TreePanel TreePanel = CreateTreePanel();
    public readonly MenuStrip MainMenu;
    public readonly Form VersionDialog;
    public readonly SplitContainer SplitContainer;
    public readonly ToolStripContainer ToolStripContainer = new ToolStripContainer();
    public readonly StatusBar StatusBar = new StatusBar();


    public MainForm( string title, Form versionDlg )
    {
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

    public MainForm( string title )
      : this( title, new AboutBox() )
    { }

    public MainForm()
      : this( "�q�X�C [������ЃJ�^�b�`]" )
    { }


    public static ViewPanel CreateViewPanel()
    {
      //ViewPanel view = new QuadViewPanel() { Dock = DockStyle.Fill };
      ViewPanel view = new SingleViewPanel() { Dock = DockStyle.Fill };
      view.SetUp( Ctrl.Current.DocumentViews );
      return view;
    }


    public static TreePanel CreateTreePanel()
    {
      TreePanel tree = new TreePanel() { Dock = DockStyle.Fill };
      tree.DocumentViews = Hisui.SI.DocumentViews;
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
      ReadSettings( form, Properties.Settings.Default );
      form.FormClosing += ( sender, e ) => WriteSettings( form, Properties.Settings.Default );
    }


    static void WriteSettings( Form form, Properties.Settings settings )
    {
      // DataBindings �𗘗p����ƃE�B���h�E�̍ő剻/�ŏ������s�����Ƃ���
      // ���삪���������Ȃ��Ă��܂��̂ŁADataBindings �𗘗p�����Ɏ����ŏ������ށB
      settings.MainFormMaximized = (form.WindowState == FormWindowState.Maximized);
      if ( form.WindowState == FormWindowState.Normal ) {
        settings.MainFormClientSize = form.ClientSize;
        settings.MainFormLocation = form.Location;
      }
      settings.Save();
    }

    static void ReadSettings( Form form, Properties.Settings settings )
    {
      form.WindowState = settings.MainFormMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
      var rect = new Rectangle( settings.MainFormLocation, settings.MainFormClientSize );
      if ( Screen.AllScreens.Any( screen => screen.Bounds.IntersectsWith( rect ) ) ) {
        form.StartPosition = FormStartPosition.Manual;
        form.Location = rect.Location;
        form.ClientSize = rect.Size;
      }
      else {
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }
  }
}
