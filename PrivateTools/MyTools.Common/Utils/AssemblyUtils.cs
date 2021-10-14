using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common.Utils
{
    using System.Reflection;

    public class AssemblyUtils
    {
        /// <summary>
        /// クラスの完全厳密名からインスタンスを作成します
        /// </summary>
        /// <param name="assemblyLib">アセンブリオブジェクト</param>
        /// <param name="className">クラスの完全厳密名</param>
        /// <param name="args">パラメータ</param>
        /// <returns>インスタンス</returns>
        public static dynamic CreateInstance(Assembly assemblyLib, string className, params object[] args)
        {
            var classType = assemblyLib.GetType(className);
            return Activator.CreateInstance(classType, args);
        }

        /// <summary>
        /// 実行ディレクトリに存在するアセンブリを読み込みます
        /// </summary>
        /// <param name="assemblyName">アセンブリ名</param>
        /// <returns>アセンブリオブジェクト</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom", Justification = "レビュー済み")]
        public static Assembly LoadAssembly(string assemblyName)
        {
            var appPath = PathUtils.GetAppPath();
            var assemblyPath = System.IO.Path.Combine(appPath, assemblyName);
            if (!System.IO.File.Exists(assemblyPath))
            {
                return null;
            }

            return Assembly.LoadFrom(assemblyPath);
        }
    }
}
