using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputField_override : InputField {

    private Event m_ProcessingEvent = new Event();

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (!isFocused)
            return;

        bool consumedEvent = false;
        while (Event.PopEvent(m_ProcessingEvent))
        {
            if (m_ProcessingEvent.rawType == EventType.KeyDown)
            {
                consumedEvent = true;
                var shouldContinue = KeyPressed(m_ProcessingEvent);
                if (shouldContinue == EditState.Finish)
                {
                    DeactivateInputField();
                    break;
                }
            }

            switch (m_ProcessingEvent.type)
            {
                case EventType.ValidateCommand:
                case EventType.ExecuteCommand:
                    switch (m_ProcessingEvent.commandName)
                    {
                        case "SelectAll":
                            SelectAll();
                            consumedEvent = true;
                            break;
                    }
                    break;
            }
        }

        if (consumedEvent)
            UpdateLabel();

        eventData.Use();
    }
}
