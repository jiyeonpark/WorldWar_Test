using Viveport;

public class ViveportIAPScript : StoreScript
{
    public ViveportIAPScript(StoreScriptId Id, object model) : base(Id, model) { }

    private ViveportIAPurchaseListener listener;

    public override void Init()
    {
        listener = new ViveportIAPurchaseListener();

        IAPurchase.IsReady(listener, ViveportStore.APP_KEY);
    }

    public void Request()
    {
        listener.item.items = new string[3];
        listener.item.items[0] = "sword";
        listener.item.items[1] = "knife";
        listener.item.items[2] = "medicine";
        IAPurchase.Request(listener, "1");
    }

    public void Purchase()
    {
        IAPurchase.Purchase(listener, listener.item.ticket);
    }

    public void Query()
    {
        IAPurchase.Query(listener, listener.item.ticket);
    }

    public void GetBalance()
    {
        IAPurchase.GetBalance(listener);
    }

    class Item
    {
        public string ticket = "test_id";
        public string[] items;
        public string subscription_ticket = "unity_test_subscriptionId";
    }

    class ViveportIAPurchaseListener : IAPurchase.IAPurchaseListener
    {
        public Item item = new Item();
        public override void OnSuccess(string pchCurrencyName)
        {
            Viveport.Core.Logger.Log("[OnSuccess] pchCurrencyName=" + pchCurrencyName);
        }

        public override void OnRequestSuccess(string pchPurchaseId)
        {
            item.ticket = pchPurchaseId;
            Viveport.Core.Logger.Log("[OnRequestSuccess] pchPurchaseId=" + pchPurchaseId + ",item.ticket=" + item.ticket);
        }

        public override void OnPurchaseSuccess(string pchPurchaseId)
        {
            Viveport.Core.Logger.Log("[OnPurchaseSuccess] pchPurchaseId=" + pchPurchaseId);
            if (item.ticket == pchPurchaseId)//if stored id equals the purchase id which is returned by OnPurchaseSuccess(), give the virtual items to user
            {
                Viveport.Core.Logger.Log("[OnPurchaseSuccess] give items to user");
                //to implement: give virtual items to user
            }
        }

        public override void OnQuerySuccess(IAPurchase.QueryResponse response)
        {
            //when status equals "success", then this purchase is valid, you can deliver virtual items to user
            Viveport.Core.Logger.Log("[OnQuerySuccess] purchaseId=" + response.purchase_id + ",status=" + response.status);
        }
        public override void OnBalanceSuccess(string pchBalance)
        {
            Viveport.Core.Logger.Log("[OnBalanceSuccess] pchBalance=" + pchBalance);
        }

        public override void OnRequestSubscriptionSuccess(string pchSubscriptionId)
        {
            item.subscription_ticket = pchSubscriptionId;
            Viveport.Core.Logger.Log("[OnRequestSubscriptionSuccess] pchSubscriptionId=" + pchSubscriptionId + ",item.subscription_ticket=" + item.subscription_ticket);
        }

        public override void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
        {
            item.subscription_ticket = pchSubscriptionId;
            Viveport.Core.Logger.Log("[OnRequestSubscriptionWithPlanIDSuccess] pchSubscriptionId=" + pchSubscriptionId + ",item.subscription_ticket=" + item.subscription_ticket);
        }

        public override void OnSubscribeSuccess(string pchSubscriptionId)
        {
            Viveport.Core.Logger.Log("[OnSubscribeSuccess] pchSubscriptionId=" + pchSubscriptionId);
            if (item.subscription_ticket == pchSubscriptionId)
            {
                Viveport.Core.Logger.Log("[OnSubscribeSuccess] give virtual items to user");
                //to implement: give virtual items to user
            }
        }

        public override void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
        {
            int size = subscriptionlist.Length;
            Viveport.Core.Logger.Log("[OnQuerySubscriptionSuccess] subscriptionlist size =" + size);
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    //when status equals "ACTIVE", then this subscription is valid, you can deliver virtual items to user
                    Viveport.Core.Logger.Log("[OnQuerySubscriptionSuccess] subscriptionlist[" + i + "].status =" + subscriptionlist[i].status +
                        ", subscriptionlist[" + i + "].plan_id = " + subscriptionlist[i].plan_id);
                    if (subscriptionlist[i].plan_id == "pID" && subscriptionlist[i].status == "ACTIVE")
                    {
                        //bIsDuplicatedSubscription = true;
                    }
                }
            }
        }

        public override void OnCancelSubscriptionSuccess(bool bCanceled)
        {
            Viveport.Core.Logger.Log("[OnCancelSubscriptionSuccess] bCanceled=" + bCanceled);
        }

        public override void OnFailure(int nCode, string pchMessage)
        {
            Viveport.Core.Logger.Log("[OnFailed] " + nCode + ", " + pchMessage);
        }
    }
}
