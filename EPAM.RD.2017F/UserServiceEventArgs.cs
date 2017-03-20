using EPAM.RD._2017F.Entities;
using System;

namespace EPAM.RD._2017F
{
    public sealed class UserServiceAddEventArgs : EventArgs
    {
        private readonly User user;

        public UserServiceAddEventArgs(User user)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public User User { get { return user; } }
    }

    public sealed class UserServiceRemoveEventArgs : EventArgs
    {
        private readonly User user;

        public UserServiceRemoveEventArgs(User user)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public User User { get { return user; } }
    }
}
