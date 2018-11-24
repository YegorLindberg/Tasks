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
            for (int i = 0; i < args.Length; i++)
            {
                enterLine += args[i];
            }
            Console.WriteLine(enterLine);

            //MARK: conversion to Postfix form
            string value = "";
            for (int i = 0; i < enterLine.Length; i++)
            {
                switch (enterLine[i])
                {
                    case char ch when IsDigit(ch):
                        value += enterLine[i];
                        break;
                    case char ch when IsOperation(ch):
                        if (value != "")
                        {
                            result.Add(value);
                            value = "";
                        }
                        determinesStack(ref result, ref stack, enterLine[i]);
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
            for (int i = 0; i < result.Count; i++)
            {
                switch (result[i])
                {
                    case string str when IsDigit(str[0]):
                        numStack.Push(Convert.ToDouble(str));
                        break;
                    case string str when (IsOperation(str[0]) && (str[0] != '(') && (str[0] != ')')):
                        calculateResult(ref numStack, str[0]);
                        break;
                    default:
                        Console.WriteLine("Error: the misspelled expression. Check the input.");
                        //Console.ReadKey();
                        return;
                }
            }
            Console.WriteLine("Ответ: " + numStack.Peek());
        }



        private static void calculateResult(ref Stack<double> ourStack, char currOperation)
        {
            try
            {
                if (ourStack.Count >= 2)
                {
                    var lastNum = ourStack.Pop();
                    var preLastNum = ourStack.Pop();
                    if ((lastNum == 0) && (currOperation == '/'))
                    {
                        throw new Exception("To zero cannot be split.");
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
