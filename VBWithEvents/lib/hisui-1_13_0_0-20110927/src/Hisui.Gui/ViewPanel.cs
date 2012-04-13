using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Hisui.Gui
{
  /// <summary>
  /// �r���[����p�c�[���o�[�� <see cref="Graphics.IView"/> ���ڂ����R���g���[���ł��B
  /// </summary>
  public partial class ViewPanel : UserControl, Core.IBuild
  {
    Core.Breath _breath = new Core.Breath();
    Graphics.DocumentViews _docviews;
    Func<ContextMenuStrip> _contextMenuFactory;

    protected void AddView( Graphics.IView view )
    {
      var setting = ViewSetting.Get( _docviews );
      if ( setting.ShowCompass ) CompassScene.PutTo( view );  // �R���p�X
      if ( setting.ShowRuler ) RulerScene.PutTo( view );      // ���[���[
      _docviews.AddView( view, this.ViewSetting.CreateViewOperation( () => _contextMenuFactory() ) );
    }

    protected void AddView( Graphics.IView view, int[] targetPath )
    {
      this.AddView( view );
      _docviews.ActiveViewTargetPath = targetPath;
    }

    public ViewPanel()
    {
      InitializeComponent();

      /*
       * btnFit �� DropDownMenu �� CodAxis ���֘A�t��
       */
      menuFitXY.Tag = Geom.CodAxis.Z;
      menuFitYZ.Tag = Geom.CodAxis.X;
      menuFitZX.Tag = Geom.CodAxis.Y;

      /*
       * btnClipOnOff �� DropDownMenu �� ClipDirections ���֘A�t��
       */
      menuClipView.Tag = ClipDirections.View;
      menuClipPlusX.Tag = ClipDirections.PlusX;
      menuClipPlusY.Tag = ClipDirections.PlusY;
      menuClipPlusZ.Tag = ClipDirections.PlusZ;
      menuClipMinusX.Tag = ClipDirections.MinusX;
      menuClipMinusY.Tag = ClipDirections.MinusY;
      menuClipMinusZ.Tag = ClipDirections.MinusZ;

      /*
       * �{�^���� PolygonStyles ���֘A�t��
       */
      btnStyleFace.Tag = Graphics.PolygonStyles.Face;
      btnStyleEdge.Tag = Graphics.PolygonStyles.Edge;
      btnStyleFaceEdge.Tag = Graphics.PolygonStyles.Face | Graphics.PolygonStyles.Edge;

      /*
       * �����̐ݒ�
       */
      Graphics.Light light = Graphics.Light.Lights[0];
      light.Enabled = true;
      light.Position = new Geom.HmCod3d( -1, 1, 1, 0 );
      light.AmbientColor = Color.FromArgb( 0x80, 0x80, 0x80 );
      light.DiffuseColor = Color.FromArgb( 0x80, 0x80, 0x80 );
      light.SpecularColor = Color.FromArgb( 0x80, 0x80, 0x80 );
    }

    /// <summary>
    /// �������֐��B�r���[�̕\���ΏۂƂȂ� <see cref="Graphics.DocumentViews"/> ���w�肵�܂��B
    /// ���̃R���g���[�����j�������ۂ� <see cref="ViewSetting"/> ���ۑ������悤�ɐݒ肳��܂��B
    /// </summary>
    /// <param name="docviews">�r���[�̕\���Ώ�</param>
    public virtual void SetUp( Graphics.DocumentViews docviews )
    {
      /*
       *  �R���e�L�X�g���j���[�����֐��̐ݒ�
       */
      _contextMenuFactory = SI.CreateContextMenu;

      /*
       *  DocumentViews �ݒ�
       */
      _docviews = docviews;
      _docviews.ViewHistoryChanged +=
        delegate( object sender, EventArgs e )
        {
          btnPrev.Enabled = _docviews.ActiveHistory.HasPrev;
          btnNext.Enabled = _docviews.ActiveHistory.HasNext;
        };

      /*
       *  �I������ ViewSetting ��ۑ�
       */
      this.Disposed += ( sender, e ) => ViewSetting.Save( _docviews );

      /*
       *  ���C�A�E�g�̍X�V
       */
      this.UpdateLayout();
    }

    protected virtual void UpdateLayout( Rectangle client ) { }

    void UpdateLayout()
    {
      int y = this.ToolStripVisible ? toolStrip1.Height : 0;
      this.UpdateLayout(
        new Rectangle( new Point( 0, y ), new Size( this.Width, this.Height - y ) ) );
    }

    /// <summary>
    /// �E�N���b�N�ŕ\�������R���e�L�X�g���j���[�𐶐�����t�@�N�g���֐��� set/get ���܂��B
    /// �f�t�H���g�ł� <see cref="SI.CreateContextMenu()"/> ���ݒ肳��Ă��܂��B
    /// </summary>
    [Browsable( false )]
    [DefaultValue( null )]
    public Func<ContextMenuStrip> ContextMenuFactory
    {
      get { return _contextMenuFactory; }
      set { _contextMenuFactory = value; }
    }

    /// <summary>
    /// �r���[�̐ݒ�����擾���܂��B
    /// </summary>
    [Browsable( false )]
    [DefaultValue( null )]
    public ViewSetting ViewSetting
    {
      get { return ViewSetting.Get( _docviews ); }
    }

    /// <summary>
    /// �c�[���o�[�̕\��ON/OFF��Ԃ� set/get ���܂��B
    /// </summary>
    [DefaultValue( true )]
    public bool ToolStripVisible
    {
      get { return toolStrip1.Visible; }
      set { toolStrip1.Visible = value; if ( this.Created ) this.UpdateLayout(); }
    }

    /// <summary>
    /// �|�y���ꂽ <see cref="Graphics.DocumentViews"/> ��Ԃ��܂��B
    /// </summary>
    public Graphics.DocumentViews DocumentViews
    {
      get { return _docviews; }
    }

    public IEnumerable<Core.IBreath> Sources
    {
      get
      {
        yield return this.ViewSetting;
        yield return _docviews;
      }
    }

    public void Build()
    {
      // �|���S���`��X�^�C��
      foreach ( var button in
        new ToolStripButton[] { btnStyleFace, btnStyleEdge, btnStyleFaceEdge } ) {
        var style = (Graphics.PolygonStyles)button.Tag;
        button.Checked = (style == this.ViewSetting.PolygonStyle);
      }

      // �������e / ���s���e
      btnPerspective.Checked = this.ViewSetting.Perspective;
      btnOrtho.Checked = !this.ViewSetting.Perspective;

      // �������`�� / �\���̂ݕ`��
      btnVisibleSideBoth.Checked = this.ViewSetting.Backside;
      btnVisibleSideFront.Checked = !this.ViewSetting.Backside;
    }

    public int BreathCount
    {
      get { return _breath.BreathCount; }
    }

    public void Touch()
    {
      _breath.Touch();
    }

    private void btnPerspective_Click( object sender, EventArgs e )
    {
      this.ViewSetting.Perspective = true;
      this.Build();
      _docviews.Invalidate();
    }

    private void btnOrtho_Click( object sender, EventArgs e )
    {
      this.ViewSetting.Perspective = false;
      this.Build();
      _docviews.Invalidate();
    }

    private void btnFit_ButtonClick( object sender, EventArgs e )
    {
      _docviews.Fit();
    }

    private void btnStyle_ButtonClick( object sender, EventArgs e )
    {
      var button = (ToolStripButton)sender;
      this.ViewSetting.PolygonStyle = (Graphics.PolygonStyles)button.Tag;
      this.Build();
      _docviews.Invalidate();
    }

    private void btnFit_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      _docviews.Fit( (Geom.CodAxis)e.ClickedItem.Tag );
    }

    private void btnSetting_Click( object sender, EventArgs e )
    {
      ViewSettingDialog dialog = new ViewSettingDialog( _docviews, this.ViewSetting );
      dialog.Owner = this.FindForm();
      dialog.StartPosition = FormStartPosition.Manual;

      Rectangle r = RectangleToScreen( ClientRectangle );
      dialog.Top = r.Top;
      dialog.Left = r.Left;

      dialog.Show();
    }

    private void btnPrev_Click( object sender, EventArgs e )
    {
      _docviews.ActiveHistory.Prev();
    }

    private void btnNext_Click( object sender, EventArgs e )
    {
      _docviews.ActiveHistory.Next();
    }

    private void btnSaveImage_Click( object sender, EventArgs e )
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Title = "�r���[���摜�Ƃ��ĕۑ�";
      dialog.Filter = "PNG files (*.png)|*.png|JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*";
      if ( dialog.ShowDialog() == DialogResult.OK ) {
        Image image = _docviews.ActiveView.GetImage();
        if ( dialog.FileName.EndsWith( "png" ) ) {
          image.Save( dialog.FileName, System.Drawing.Imaging.ImageFormat.Png );
        }
        else if ( dialog.FileName.EndsWith( "jpg" ) ) {
          image.Save( dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg );
        }
        else {
          image.Save( dialog.FileName );
        }
      }
    }

    private void btnCopyImage_Click( object sender, EventArgs e )
    {
      Image image = _docviews.ActiveView.GetImage();
      Clipboard.SetDataObject( image );
    }

    private void btnVisibleSideBoth_Click( object sender, EventArgs e )
    {
      this.ViewSetting.Backside = true;
      this.Build();
      _docviews.Invalidate();
    }

    private void btnVisibleSideFront_Click( object sender, EventArgs e )
    {
      this.ViewSetting.Backside = false;
      this.Build();
      _docviews.Invalidate();
    }

    private void btnEnvInfo_Click( object sender, EventArgs e )
    {
      new EnvInfoDialog().ShowDialog();
    }

    private void btnClipOnOff_ButtonClick( object sender, EventArgs e )
    {
      if ( Gui.ClipPlane.Find( SI.DocumentViews ) != null )
        Gui.ClipPlane.Remove( SI.DocumentViews );
      else
        Gui.ClipPlane.Get( SI.DocumentViews );

      var dir = Gui.ClipPlane.GetClipDirection( SI.DocumentViews );
      foreach ( ToolStripMenuItem menu in btnClipOnOff.DropDownItems ) {
        menu.Checked = ((ClipDirections)menu.Tag == dir);
      }
      _docviews.Invalidate();
    }

    private void btnClipOnOff_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      var dir = (ClipDirections)e.ClickedItem.Tag;
      Gui.ClipPlane.SetClipDirection( SI.DocumentViews, dir );
      foreach ( ToolStripMenuItem menu in btnClipOnOff.DropDownItems ) {
        menu.Checked = ((ClipDirections)menu.Tag == dir);
      }
      _docviews.Invalidate();
    }
  }
}
