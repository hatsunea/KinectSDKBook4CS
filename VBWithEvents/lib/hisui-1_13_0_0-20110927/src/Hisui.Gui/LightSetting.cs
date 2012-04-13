using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Hisui.Gui
{
  /// <summary>
  /// �����ݒ��\���N���X�ł��B
  /// <see cref="Graphics.Light"/> �N���X�̃��b�p�[�ł��B
  /// </summary>
  [TypeConverter( typeof( ExpandableObjectConverter ) )]
  public class LightSetting
  {
    Graphics.Light _light;

    /// <summary>
    /// �R���X�g���N�^�B
    /// ���b�v�Ώۂ̌������w�肵�č\�z���܂��B
    /// </summary>
    /// <param name="light">���b�v�Ώۂ̌���</param>
    public LightSetting( Graphics.Light light )
    {
      _light = light;
    }

    /// <summary>
    /// ������ON/OFF�� set/get ���܂��B
    /// </summary>
    public bool Enabled
    {
      get { return _light.Enabled; }
      set { _light.Enabled = value; }
    }

    /// <summary>
    /// ���������[���h���W�n�ɒu�����J�������W�n�ɒu������ set/get ���܂��B
    /// <see cref="Position"/>�v���p�e�B�����[���h���W�n�̍��W�Ƃ݂Ȃ����A�J�������W�n�̍��W�Ƃ݂Ȃ����A���ݒ�ł��܂��B
    /// </summary>
    public bool IsWorldCoordinate
    {
      get { return _light.IsWorldCoordinate; }
      set { _light.IsWorldCoordinate = value; }
    }

    /// <summary>
    /// ������ set/get ���܂��B
    /// </summary>
    public Color AmbientColor
    {
      get { return _light.AmbientColor; }
      set { _light.AmbientColor = value; }
    }

    /// <summary>
    /// �g�U���� set/get ���܂��B
    /// </summary>
    public Color DiffuseColor
    {
      get { return _light.DiffuseColor; }
      set { _light.DiffuseColor = value; }
    }

    /// <summary>
    /// ���ˌ��� set/get ���܂��B
    /// </summary>
    public Color SpecularColor
    {
      get { return _light.SpecularColor; }
      set { _light.SpecularColor = value; }
    }

    /// <summary>
    /// �����̈ʒu�𓯎����W�� set/get ���܂��B
    /// <c>Position.w == 0</c> �̏ꍇ�͖������_�ɒu���ꂽ�����Ɖ��߂���A���s�����ƂȂ�܂��B
    /// <c>Position.w != 0</c> �̏ꍇ�͓_�����ƂȂ�܂��B
    /// </summary>
    public Geom.HmCod3d Position
    {
      get { return _light.Position; }
      set { _light.Position = value; }
    }
  }
}
