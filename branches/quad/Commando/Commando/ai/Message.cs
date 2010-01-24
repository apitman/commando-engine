using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando.ai
{
    public class Message
    {
        /// <summary>
        /// This represents the number of frames it takes to send
        /// one character in a message. It is the framerate divided
        /// by the number of letters one can say per second.
        /// TODO: In the future, this should depend on the number of
        /// syllables instead of the number of letters in the message.
        /// </summary>
        private const int TIME_MULTIPLIER = 30 / 5;

        protected static int NextId = 0;

        internal int Id_ { get; private set; }

        /// <summary>
        /// The belief (data) passed in the message (if there is any).
        /// </summary>
        internal Belief Belief_ { get; set; }
        
        /// <summary>
        /// Can be Hi, Data, or Bye
        /// </summary>
        internal MessageType MessageType_ { get; set; }

        /// <summary>
        /// The constructor to use
        /// </summary>
        /// <param name="mT">The type of the Message</param>
        internal Message(MessageType mT)
        {
            MessageType_ = mT;
            Id_ = NextId++;
        }

        /// <summary>
        /// Gets the number of frames it takes to broadcast this message.
        /// </summary>
        /// <returns>The number of frames it takes to broadcast this message.</returns>
        internal int TimeToBroadcast()
        {
            return this.ToString().Length * TIME_MULTIPLIER;
        }

        public override string ToString()
        {
            if (MessageType_ == MessageType.Hi)
            {
                return "Hi";
            }
            else if (MessageType_ == MessageType.Bye)
            {
                return "Bye";
            }
            else if (MessageType_ == MessageType.Data)
            {
                return Belief_.ToString();
            }
            else
            {
                return "DEFAULT MESSAGE";
            }
        }

        /// <summary>
        /// Different MessageTypes are handled differently.
        /// Data is used for broadcasting Beliefs.
        /// </summary>
        internal enum MessageType
        {
            Hi,
            Data,
            Bye
        }
    }
}
