using System;

namespace DotEngine.AI.FSM
{
    public class StateToken : Token
    {
        public StateToken():base()
        {
        }

        public StateToken(string name):base(name)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ActionToken:Token
    {
        public ActionToken() : base()
        {
        }

        public ActionToken(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// 使用guid做为唯一的标识。
    /// </summary>
    public abstract class Token : IEquatable<Token>
    {
        private Guid guid;
        private string name;

        public string Name => name;

        protected Token()
        {
            guid = Guid.NewGuid();
        }

        protected Token(string name) : this()
        {
            if(name == null)
            {
                throw new ArgumentException("Token::Token->name is null");
            }
            this.name = name.Trim();
            if(this.name.Length == 0)
            {
                throw new ArgumentException("Token::Token->name is empty");
            }
        }

        /// <summary>
        /// 重写比较函数，guid相同则两对象相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Token other)
        {
            if(other == null)
            {
                return false;
            }

            return other.guid == guid;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Token))
            {
                return false;
            }

            return Equals((Token)obj);
        }

        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }

        public static bool operator == (Token a,Token b)
        {
            if (object.ReferenceEquals(a, b))
                return true;

            if ((object)a == null || (object)b == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator != (Token a,Token b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return name??"(unnamed token)";
        }
    }
}
