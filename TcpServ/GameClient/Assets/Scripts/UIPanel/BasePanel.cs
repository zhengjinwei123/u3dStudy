using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {

	protected UIManager uiMgr;

	protected GameFacade facade;

	public UIManager UIMgr {
		set { uiMgr = value;  }
	}

	public GameFacade Facade {
		set { facade = value;  }
		get { return facade;  }
	}

    /// <summary>
    /// 界面被显示出来
    /// </summary>
	/// 
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {

    }

	protected void PlayClickSound() {
		Facade.PlayNormalSound(AudioManager.Sound_ButtonClick);
	}
}
