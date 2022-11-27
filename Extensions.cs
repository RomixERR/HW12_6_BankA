﻿using System.Collections.Generic;

namespace HW12_6_BankA
{
    /// <summary>
    /// Метод расширения позволяет "добавить" методы в существующие типы без создания нового производного типа
    /// В данном случае добавлен метод find в List<Client>
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Моё расширение для поиска [индекса] клиента
        /// </summary>
        /// <typeparam name="Client"></typeparam>
        /// <param name="list"></param>
        /// <param name="target">Клиент которого нужно найти</param>
        /// <returns></returns>
        public static int find<Client>(this List<Client> list, Client target)
        {
            return list.IndexOf(target);
        }
    }
}
