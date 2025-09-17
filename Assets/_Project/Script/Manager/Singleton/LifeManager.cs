using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : SingletonGeneric<LifeManager>
{
    protected override bool ShouldBeDestroyOnLoad() => false;

    private bool _isFirstLevel;
    private int _key;
    private LifeController _playerLifeController;
    private int _hp;
    private int _hpStartLevel;

    public void SetIsFirstLevel() => _isFirstLevel = true;

    //Awake chiamato dal LifeController
    public void SetLifeController(int key, LifeController lifeController)
    {
        if (_playerLifeController == null)
        {
            _key = key;
            _playerLifeController = lifeController;
            if (_isFirstLevel)
            {
                SetHp();
            }
            else
            {
                _playerLifeController.SetHp(_key, _hpStartLevel);
            }
            _playerLifeController.onChangeHp.AddListener((int hp, int maxHp) => SetHp());
        }
    }

    public void SetHp() => _hp = _playerLifeController.GetHp();

    //Start chiamato dal LevelSettings
    public void OnPlayerStartLevel()
    {
        if (_playerLifeController != null)
        {
            
        }
    }

    private void OnPlayerChangeLevel()
    {
        _isFirstLevel = false;
    }

    public void OnPlayerReloadLevel()
    {
        _playerLifeController = null;
        _hp = _hpStartLevel;
        OnPlayerChangeLevel();
    }

    public void OnPlayerWinLevel()
    {
        _playerLifeController = null;
        _hpStartLevel = _hp;
        OnPlayerChangeLevel();
    }

    public void OnPlayerReturnMainMenu()
    {
        _hp = 0;
        _hpStartLevel = 0;
        OnPlayerChangeLevel();
    }
}
