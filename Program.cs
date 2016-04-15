using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;

class Solution
{
    class MinHeap<T> where T : IComparable
    {
        List<T> elements;

        public int Count 
        {
            get { return elements.Count; }
        }

        public MinHeap()
        {
            elements = new List<T>();
        }

        public void Add(T item)
        {
            elements.Add(item);
            Heapify();
        }

        public void Delete(T item)
        {
            int i = elements.IndexOf(item);
            int last = elements.Count - 1;

            elements[i] = elements[last];
            elements.RemoveAt(last);
            Heapify();
        }

        public T PopMin()
        {
            if (elements.Count > 0)
            {
                T item = elements[0];
                Delete(item);
                return item;
            }
            //relook at this - should we just throw exception?
            return default(T);
        }

        public void Heapify()
        {
            for (int i = elements.Count - 1; i > 0; i--)
            {
                int parentPosition = (i + 1) / 2 - 1;
                parentPosition = parentPosition >= 0 ? parentPosition : 0;

                if (elements[parentPosition].CompareTo(elements[i]) > 0)
                {
                    T tmp = elements[parentPosition];
                    elements[parentPosition] = elements[i];
                    elements[i] = tmp;
                }
            }
        }
    }

    struct Position : IComparable
    {
        public bool Equals(Position other)
        {
            return A == other.A && B == other.B && C == other.C && D == other.D;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = A;
                hashCode = (hashCode*397) ^ B;
                hashCode = (hashCode*397) ^ C;
                hashCode = (hashCode*397) ^ D;
                return hashCode;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        public string Path;
        public int A;
        public int B;
        public int C;
        public int D;
        public int CompareTo(Position other)
        {
            var dx = 40 - this.A - this.B - this.C - this.D;
            var dy = 40 - other.A - other.B - other.C - other.D;
            return dx - dy;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Position) obj);
        }
    }

    static byte GetValue(byte[] bytes,Position p)
    {
        return bytes[(p.A-1)*1000 + (p.B-1)*100 + (p.C-1)*10 + (p.D-1)];
    }

    static void Main(String[] args)
    {
        var path = @"C:\sandbox projects\sandbox2\sandbox2\sandbox2\bin\Debug\mz.dat";
        var bytes = File.ReadAllBytes(path);

        Position initial;
        initial.Path = "";
        initial.A = initial.B = initial.C = initial.D = 1;

        var list = new MinHeap<Position>();
        var past = new HashSet<Position>();
        list.Add(initial);

        while (list.Count != 0)
        {
            var currentPosition = list.PopMin();
            if(currentPosition.A == 10 && currentPosition.B == 10 && currentPosition.C == 10 && currentPosition.D == 10)
            {
                Console.WriteLine(currentPosition.Path);
                break;
            }
            if (past.Contains(currentPosition))
            {
                continue;
            }
            else
            {
                past.Add(currentPosition);
            }

            var val = GetValue(bytes, currentPosition);
            if((val & 128) == 128)
            {
                var newPosition = currentPosition;
                newPosition.A--;
                newPosition.Path += "w";
                list.Add(newPosition);
            }
            if ((val & 64) == 64)
            {
                var newPosition = currentPosition;
                newPosition.A++;
                newPosition.Path += "e";
                list.Add(newPosition);
            }
            if ((val & 32) == 32)
            {
                var newPosition = currentPosition;
                newPosition.B--;
                newPosition.Path += "n";
                list.Add(newPosition);
            }
            if ((val & 16) == 16)
            {
                var newPosition = currentPosition;
                newPosition.B++;
                newPosition.Path += "s";
                list.Add(newPosition);
            }
            if ((val & 8) == 8)
            {
                var newPosition = currentPosition;
                newPosition.C--;
                newPosition.Path += "u";
                list.Add(newPosition);
            }
            if ((val & 4) == 4)
            {
                var newPosition = currentPosition;
                newPosition.C++;
                newPosition.Path += "d";
                list.Add(newPosition);
            }
            if ((val & 2) == 2)
            {
                var newPosition = currentPosition;
                newPosition.D--;
                newPosition.Path += "b";
                list.Add(newPosition);
            }
            if ((val & 1) == 1)
            {
                var newPosition = currentPosition;
                newPosition.D++;
                newPosition.Path += "f";
                list.Add(newPosition);
            }

            
        }

        Console.ReadLine();
    }


}