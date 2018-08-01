using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using CommChannel;

namespace CommChannel
{
    [ServiceContract]
    public interface ICommunicator
    {
        [OperationContract(IsOneWay = true)]
        void PostMessage(Message msg);

        // used only locally so not exposed as service method

        Message GetMessage();
    }

    // The class Message is defined in CommChannelDemo.Messages as [Serializable]
    // and that appears to be equivalent to defining a similar [DataContract]

}