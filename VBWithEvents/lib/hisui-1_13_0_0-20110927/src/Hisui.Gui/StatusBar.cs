using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Hisui.Gui
{
  /// <summary>
  /// �v���O���X�o�[��\������X�e�[�^�X�o�[�ł��B
  /// <see cref="SI.ProgressChanged"/> �C�x���g���󂯎���ăv���O���X�o�[�̕\�����X�V���܂��B
  /// </summary>
  public class StatusBar : StatusStrip
  {
    readonly ToolStripLabel _label = new ToolStripLabel();
    readonly ToolStripProgressBar _progressBar = new ToolStripProgressBar();

    public int CompletionElapsedMilliseconds = 200;
    public int CompletionSleepMilliseconds = 200;

    public StatusBar()
    {
      this.Items.Add( _progressBar );
      this.Items.Add( _label );
      _progressBar.Minimum = 0;
      _progressBar.Maximum = 100;
      _progressBar.Step = 1;
      _progressBar.Size = new Size( 150, _progressBar.Height );

      // Core.Progress �̃n���h���ݒ�
      var stopwatch = new System.Diagnostics.Stopwatch();
      SI.ProgressChanged +=
        delegate( object sender, Core.ProgressEventArgs e )
        {
          if ( e.Value >= 1.0 ) {
            if ( stopwatch.ElapsedMilliseconds > this.CompletionElapsedMilliseconds ) {
              // 100% �ɂȂ����v���O���X�o�[����u�\��
              _progressBar.Value = 100;
              this.Refresh();
              System.Threading.Thread.Sleep( this.CompletionSleepMilliseconds );
            }
            this.SetCursor( Cursors.Default );
            stopwatch.Reset();
            _progressBar.Value = 0;
            _label.Text = "";
          }
          else {
            if ( !stopwatch.IsRunning ) stopwatch.Start();
            this.SetCursor( Cursors.WaitCursor );
            _progressBar.Value = (int)(100 * e.Value);
            _label.Text = e.Caption;
            Application.DoEvents();
          }
          SI.UpdateToolStrips();
          SI.DocumentViews.Invalidate();
        };
    }

    void SetCursor( Cursor cursor )
    {
      Form form = this.FindForm();
      if ( form != null ) form.Cursor = cursor;
    }
  }
}
