using System.Windows.Forms ;
using System.Collections.Generic ;
using System.Text;
using System;
using System.Linq;

namespace Hisui.Std
{
  [Ctrl.Command]
  static partial class Menu
  {
    [Ctrl.Command( "�폜" )]
    static void Delete( IEnumerable<Core.IEntry> self, Ctrl.IContext con )
    {
      // ���Fself.ToArray() ���ăV�[�P���X���R�s�[���Ă����Ȃ��� Remove �ŗ����� 
      foreach ( var item in self.ToArray() ) item.Owner.Remove( item.ID );
    }

    [Ctrl.Command( "�N���A" )]
    public static void Clear( Core.Document self )
    {
      Ctrl.CommandHelper.Clear();
    }

    //[Ctrl.Command( "�\��/��\��" )]
    static void ShowHide( Graphics.ISceneDecorator deco, Ctrl.IContext con, Ctrl.CommandOption opt )
    {
      if ( opt.QueryRunnable ) { opt.IsChecked = deco.Visible; return; }
      deco.Visible = !deco.Visible;
    }

    [Ctrl.Command( "�L��/����" )]
    static void EnableDisable( Core.IEntry entry, Ctrl.IContext con, Ctrl.CommandOption opt )
    {
      if ( opt.QueryRunnable ) { opt.IsChecked = entry.Enabled; return; }
      entry.Enabled = !entry.Enabled;
    }

    [Ctrl.Command( "�A�N�e�B�u�t�H���_�ɐݒ�" )]
    static void Activate( Core.IEntry self, Ctrl.IContext con )
    {
      con.Document.ActiveEntries = self.Entries;
    }
  }
}

