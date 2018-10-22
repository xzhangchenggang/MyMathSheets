﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MyMathSheets.CommonLib.Message;
using MyMathSheets.CommonLib.Properties;
using MyMathSheets.CommonLib.Util;

namespace MyMathSheets.CommonLib.Composition
{
    /// <summary>
    /// 
    /// </summary>
    public static class ComposerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private static Assembly TempAssembly;

        /// <summary>
        /// 
        /// </summary>
        private static readonly ConcurrentDictionary<SystemModel, Composer> ComposerCache;

        /// <summary>
        /// 
        /// </summary>
        static ComposerFactory()
        {
            ComposerCache = new ConcurrentDictionary<SystemModel, Composer>();
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        /// 
        /// </summary>
        public static void Init()
        {
            var action = new Action<FileInfo>(f =>{
                var assembly = Assembly.LoadFile(f.FullName);
				MathSheetMarkerAttribute attr = assembly.GetCustomAttributes(typeof(MathSheetMarkerAttribute), false)
                    .Cast<MathSheetMarkerAttribute>()
                    .FirstOrDefault();
                if(attr == null)
                {
                    throw new Exception();
                }
                var valueFunc = new Func<SystemModel, Composer>(c =>
                {
                    lock(Sync)
                    {
                        return new Composer(assembly);
                    }
                });

                ComposerCache.GetOrAdd(attr.SystemId, valueFunc);
            });

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(basePath);
			directory.GetFiles("MyMathSheets.*.dll").ToList().ForEach(f => action(f));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public static Composer GetComporser(SystemModel systemId)
        {
            if(ComposerCache.ContainsKey(systemId))
            {
				if (ComposerCache.TryGetValue(systemId, out Composer composer))
				{
					return composer;
				}
			}

            var valueFunc = new Func<SystemModel, Composer>(c =>
            {
                lock(Sync)
                {
                    return new Composer(GetAssembly(systemId));
                }
            });

            return ComposerCache.GetOrAdd(systemId, valueFunc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        private static Assembly GetAssembly(SystemModel systemId)
        {
            var action = new Action<FileInfo>(f =>
            {
                var assembly = Assembly.LoadFile(f.FullName);
                var attr = assembly.GetCustomAttributes(typeof(MathSheetMarkerAttribute), true).Cast<MathSheetMarkerAttribute>().FirstOrDefault();
                if(attr != null && systemId.Equals(attr.SystemId))
                {
                    TempAssembly = assembly;
                }
            });

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo directory = new DirectoryInfo(basePath);
            directory.GetFiles("MyMathSheets.*.dll").ToList().ForEach(f => action(f));

			if (TempAssembly == null)
            {
                var sb = new StringBuilder();
                sb.Append(MessageUtil.GetException(() => Resources.E0001L));
                throw new ComposerException(sb.ToString());
            }
            return TempAssembly;
        }

		/// <summary>
		/// アプリケーション基盤での優先順位付けされたアセンブリの順序で生成されたコンテナを返します。
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static IEnumerable<ComposablePartCatalog> GetCatalog(string path)
		{
			foreach (var fi in new DirectoryInfo(path)
						.GetFiles("MyMathSheets.*.dll")
						.OrderByDescending(_ => _.Name.Length))
			{
				yield return new DirectoryCatalog(path, fi.Name);
			}
		}

		/// <summary>
		/// UT用にコンテナーをリセットします。
		/// </summary>
		/// <remarks>
		/// UT以外では呼び出されることはありません。
		/// </remarks>
		internal static void Reset()
		{
			lock (Sync)
			{
				foreach (var keyValue in ComposerCache)
				{
					if (keyValue.Value is IDisposable value)
					{
						value.Dispose();
					}
				}
				ComposerCache.Clear();
			}
		}
	}
}
