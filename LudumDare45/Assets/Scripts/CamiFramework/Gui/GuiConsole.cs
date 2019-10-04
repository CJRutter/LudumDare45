using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamiFramwork.ConsoleUtil;
using UnityEngine.UI;
using System.Text;

namespace CamiFramwork.Gui
{
    public class GuiConsole : BaseBehaviour
    {
	    void Start()
	    {
            console = Console.Instance;

            console.OnLineAdded += Console_OnLineAdded;

            if(ExecuteButton != null)
                ExecuteButton.onClick.AddListener(ExecuteCommand);

            CommandInput.onEndEdit.AddListener(OnSubmit);
            ConsoleOpen = DisplayPanel.activeInHierarchy;
	    }

        private void OnDestroy()
        {
            Console.Instance.OnLineAdded -= Console_OnLineAdded;
        }

        void Update()
	    {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                DisplayPanel.SetActive(!DisplayPanel.activeInHierarchy);

                ConsoleOpen = DisplayPanel.activeInHierarchy;

                if(ConsoleOpen)
                {
                    CommandInput.ActivateInputField();
                    CommandInput.Select();
                }
            }
	    }

        private void OnSubmit(string text)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteCommand(text);
                CommandInput.ActivateInputField();
                CommandInput.Select();
            }
        }

        public void ExecuteCommand()
        {
            ExecuteCommand(CommandInput.text);
        }

        public void ExecuteCommand(string command)
        {
            console.ProcessLine(CommandInput.text);
        }

        private void RefreshDisplay()
        {
            if (Display == null)
                return;

            displayBuffer.Length = 0;
            foreach(StringBuilder line in console.Lines)
            {
                displayBuffer.Append(line.ToString());
            }

            Display.text = displayBuffer.ToString();
            
            Scroll.verticalScrollbar.value = 0f;
        }

        private void Console_OnLineAdded()
        {
            RefreshDisplay();
        }

        #region Properties
        #endregion Properties

        #region Fields
        public GameObject DisplayPanel;
        public Text Display;
        public InputField CommandInput;
        public Button ExecuteButton;
        public ScrollRect Scroll;

        private Console console;
        private StringBuilder displayBuffer = new StringBuilder();
        
        public static bool ConsoleOpen = false;
        #endregion Fields
    }
}