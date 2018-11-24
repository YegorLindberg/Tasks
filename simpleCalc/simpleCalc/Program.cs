using System;
using System.Collections.Generic;

namespace simpleCalc
{
    class Program
    {
        private static bool IsDigit(char ch) => (ch >= '0') && (ch <= '9');
        private static bool IsOperation(char ch) => (ch >= '(') && (ch <= '+') || (ch == '-') || (ch == '/');
        static void Main(string[] args)
        {

            var result = new List<string>();
            var stack = new List<char>();

            string enterLine = "";
            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    enterLine += args[i];
                }
                Console.WriteLine(enterLine);
            } 
            else
            {
                Console.WriteLine("Empty input string.");
                return;
            }
            

            //MARK: conversion to Postfix form
            string value = "";
           
            for (int i = 0; i < enterLine.Length; i++)
            {
                switch (enterLine[i])
                {
                    case char ch when IsDigit(ch):
                        value += enterLine[i];
                        break;
                    case char ch when (IsOperation(ch) && (ch != '-')):
                        if (value != "")
                        {
                            result.Add(value);
                            value = "";
                        }
                        determinesStack(ref result, ref stack, enterLine[i]);
                        break;
                    case char ch when ch == '-':
                        if (i > 0)
                        {
                            if (IsOperation(enterLine[i - 1]) && (enterLine[i - 1] != ')'))
                            {
                                value += ch;
                            }
                            else
                            {
                                if (value != "")
                                {
                                    result.Add(value);
                                    value = "";
                                }
                                determinesStack(ref result, ref stack, enterLine[i]);
                            }
                        }
                        else
                        {
                            value += ch;                           
                        }
                        break;
                    default:
                        Console.WriteLine("Error: discovered an unidentified symbol.");
                        //Console.ReadKey();
                        return;
                }
                if (i + 1 == enterLine.Length)
                {
                    if (value != "")
                    {
                        result.Add(value);
                    }
                    if (stack.Count != 0)
                    {
                        for (int k = stack.Count - 1; k >= 0; k--)
                        {
                            result.Add(stack[k].ToString());
                            stack.RemoveAt(k);
                        }
                    }

                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                Console.Write(result[i] + " ");
            }
            Console.WriteLine();

            //MARK: calculating
            var numStack = new Stack<double>();
            bool flag = false;
            for (int i = 0; i < result.Count && flag == false; i++)
            {
                switch (result[i])
                {
                    case string str when (IsDigit(str[0]) || (str.Length > 1)):
                        numStack.Push(Convert.ToDouble(str));
                        break;
                    case string str when (IsOperation(str[0]) && (str[0] != '(') && (str[0] != ')')):
                        calculateResult(ref numStack, ref flag, str[0]);
                        break;
                    default:
                        Console.WriteLine("Error: the misspelled expression. Check the input.");
                        //Console.ReadKey();
                        return;
                }                
            }
            if (flag == false)
            {
                Console.WriteLine($"Ответ: {numStack.Peek()}");
            }
            else
            {
                Console.WriteLine("All's bad, look for error in input.");
            }
            


        }



        private static void calculateResult(ref Stack<double> ourStack, ref bool flag, char currOperation)
        {
            try
            {
                if (ourStack.Count >= 2)
                {
                    var lastNum = ourStack.Pop();
                    var preLastNum = ourStack.Pop();
                    if ((lastNum == 0) && (currOperation == '/'))
                    {
                        flag = true;
                        throw new DivideByZeroException();
                    }
                    switch (currOperation)
                    {
                        case '+':
                            ourStack.Push(preLastNum + lastNum);
                            break;
                        case '-':
                            ourStack.Push(preLastNum - lastNum);
                            break;
                        case '*':
                            ourStack.Push(preLastNum * lastNum);
                            break;
                        case '/':
                            ourStack.Push(preLastNum / lastNum);
                            break;
                    }

                }
                else
                {
                    flag = true;
                    throw new Exception("Stack have one element.");
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }

        private static void determinesStack(ref List<string> res, ref List<char> ourStack, char currentOperation)
        {
            if ((ourStack.Count == 0) || (currentOperation == '('))
            {
                ourStack.Add(currentOperation);
            }
            else if ((currentOperation == '+') || (currentOperation == '-'))
            {
                for (int i = ourStack.Count - 1; i >= 0 && ourStack[i] != '('; i--)
                {
                    res.Add(ourStack[i].ToString());
                    ourStack.RemoveAt(i);
                }
                ourStack.Add(currentOperation);
            }
            else if ((currentOperation == '*') || (currentOperation == '/'))
            {
                for (int i = ourStack.Count - 1; i >= 0 && ourStack[i] != '('; i--)
                {
                    if ((ourStack[i] == '*') || (ourStack[i] == '/'))
                    {
                        res.Add(ourStack[i].ToString());
                        ourStack.RemoveAt(i);
                    }
                }
                ourStack.Add(currentOperation);
            }
            else if (currentOperation == ')')
            {
                try
                {
                    for (int i = ourStack.Count - 1; i >= 0 && ourStack[i] != '('; i--)
                    {
                        Console.WriteLine("deleting element: " + ourStack[i] + " with id: " + i);
                        res.Add(ourStack[i].ToString());
                        ourStack.RemoveAt(i);
                        if ((i == 0) && (ourStack[i] != '('))
                        {
                            throw new Exception("You are missing \"(\".");
                        }
                    }
                    Console.WriteLine(ourStack[ourStack.Count - 1] + " - elmnt ( and count: " + ourStack.Count);
                    ourStack.RemoveAt(ourStack.Count - 1);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }
            }
        }
    }
}
