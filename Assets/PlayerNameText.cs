using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameText : MonoBehaviour
{
    private TMP_Text _nameText;

    private void Start()
    {
        _nameText = GetComponent<TMP_Text>();

        if (AuthManager._user != null)
        {
            _nameText.text = AuthManager._user.Email;
        }
        else
        {
            _nameText.text = "ERROR : AuthManager.User == null";
        }
        
        
    }
}
