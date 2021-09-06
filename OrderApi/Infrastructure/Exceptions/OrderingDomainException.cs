using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Exceptions
{
    public class OrderingDomainException : Exception //creating own exception, need to inherit from class Exception.
                                                     //why create own exception: because you want your own special name
                                                     //then creating 3 overloaded constructors
    {
        public OrderingDomainException() { } //default constructor, does nothing

        public OrderingDomainException(string message) : base(message) { } //if somebody creates an exception with a message(generally when you create your own exception you can create your own messege);
                                                                           //pass that messege to the base class (and tell what messege to call)

        public OrderingDomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
