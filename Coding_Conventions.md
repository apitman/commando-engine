# Coding Conventions #

## Data Members and Methods ##

Data members will be named using camel case and will be followed by an underscore. Methods will also be named using camel case.
For example:
```
public class Foo
{
  private String varName_;
  public String getVarName()
  {
    return varName_;
  }
}
```

## Local Variables ##

Local variables will be named using camel case.
For example:
```
public void functionName()
{
  int myInt;
  int anotherVariable;
  memberVariable_ += myInt / anotherVariable;
}
```

## Constants ##

Constants will be named using only capital letters.
For example:
```
static public int MAX_NUM_OBJECTS = 500;
```

## Classes ##

Class names will start with a capital letter.
For example:
```
public class FooBar
{
}
```

## Interfaces, Abstract Classes, Structs, and Enums ##

Interfaces will have "Interface" in their name.
Abstract classes will have "Abstract" in their name.
Structs will have "Struct" in their name.
Enums will have "Enum" in their name.

## Organization of a Class's Contents ##

  1. Constants
  1. Protected Data Members
  1. Private Data Members
  1. Public Data Members
  1. Constructor
  1. Protected Methods
  1. Private Methods
  1. Public Methods