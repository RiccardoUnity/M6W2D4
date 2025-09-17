using TMPro;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    private UICircleBar _circleBar;
    [SerializeField] private TMP_Text _totalCoin;

    void Awake()
    {
        _circleBar = GetComponent<UICircleBar>();
        CoinManager.Instance.onCoinPicked.AddListener(_circleBar.SetFill);
        CoinManager.Instance.onCoinPicked.AddListener(ShowTotalCoin);
    }

    private void ShowTotalCoin(int amount, int totalAmount)
    {
        _totalCoin.text = CoinManager.Instance.GetTotalPickdCoin().ToString();
    }
}
