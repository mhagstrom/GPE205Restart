using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Pawn _pawn;

    [SerializeField]
    private Slider _slider;
    
    private void Start()
    {
        _pawn.Health.HealthChanged += Health_HealthChanged;
        _slider.value = ((float)_pawn.Health.CurrentHealth / _pawn.Health.MaxHealth);
    }

    private void Health_HealthChanged(int health)
    {
        var value = ((float)health / _pawn.Health.MaxHealth);
        _slider.value = value;
    }

    private void OnDestroy()
    {
        _pawn.Health.HealthChanged -= Health_HealthChanged;
    }
}
