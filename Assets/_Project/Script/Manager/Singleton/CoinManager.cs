using System.Collections.Generic;
using UnityEngine.Events;

public class CoinManager : SingletonGeneric<CoinManager>
{
    private List<T_CoinPlate> _allCoins = new List<T_CoinPlate>();
    private int _currentLevelPickedCoins;
    private int _totalPickedCoins;
    public int GetTotalPickdCoin() => _totalPickedCoins + _currentLevelPickedCoins;

    public UnityEvent<int, int> onCoinPicked = new UnityEvent<int, int>();
    public UnityEvent onAllCoinPicked = new UnityEvent();

    protected override bool ShouldBeDestroyOnLoad() => false;

    public void AddCoinInList(T_CoinPlate coin)
    {
        if (!_allCoins.Contains(coin))
        {
            _allCoins.Add(coin);
        }
    }

    public void CoinPickUp()
    {
        ++_currentLevelPickedCoins;
        onCoinPicked?.Invoke(_currentLevelPickedCoins, _allCoins.Count);
        if (_currentLevelPickedCoins == _allCoins.Count)
        {
            onAllCoinPicked?.Invoke();
        }
    }

    public void OnPlayerStartLevel()
    {
        _currentLevelPickedCoins = 0;
        onCoinPicked?.Invoke(_currentLevelPickedCoins, _allCoins.Count);
    }

    private void OnPlayerChangeLevel()
    {
        _allCoins.Clear();
        onCoinPicked.RemoveAllListeners();
        onAllCoinPicked.RemoveAllListeners();
    }

    public void OnPlayerReloadLevel()
    {
        OnPlayerChangeLevel();
    }

    public void OnPlayerWinLevel()
    {
        _totalPickedCoins += _currentLevelPickedCoins;
        OnPlayerChangeLevel();
    }

    public void OnPlayerReturnMainMenu()
    {
        _totalPickedCoins = 0;
        OnPlayerChangeLevel();
    }
}