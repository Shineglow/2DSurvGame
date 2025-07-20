using Events;
using Player.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Inventory
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private Image itemView;
        [SerializeField] private TextMeshProUGUI count;
        
        [SerializeField] private EventSO<SlotInfo> slotUpdateEvent;

        private SlotInfo _bindedSlotInfo;

        public void Bind(SlotInfo slotInfo)
        {
            _bindedSlotInfo = slotInfo;
            OnSlotUpdate(slotInfo);
        }

        private void OnEnable()
        {
            slotUpdateEvent.Subscribe(OnSlotUpdate, 1);
        }

        private void OnDisable()
        {
            slotUpdateEvent.Unsubscribe(OnSlotUpdate);
        }

        private void OnSlotUpdate(SlotInfo slotInfo)
        {
            if (_bindedSlotInfo != slotInfo) return;

            var itemInfo = slotInfo?.AbstractItemBase;
            if (itemInfo == null)
            {
                itemView.gameObject.SetActive(false);
            }
            else
            {
                itemView.sprite = itemInfo.Sprite;
                itemView.SetNativeSize();
                itemView.gameObject.SetActive(true);
            }
            
            if (itemInfo == null)
            {
                count.gameObject.SetActive(false);
            }
            else
            {
                if (itemInfo.CanStack && itemInfo.MaxItemsInStack > 1 && slotInfo.Count > 1)
                {
                    count.text = slotInfo.Count.ToString();
                    count.gameObject.SetActive(true);
                }
                else
                {
                    count.gameObject.SetActive(false);
                }
            }
        }
    }
}
