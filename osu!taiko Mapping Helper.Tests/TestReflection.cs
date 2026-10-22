using System.Reflection;
using System.Runtime.CompilerServices;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// private のメンバーや画面のクラスをテストから扱うための補助
    /// </summary>
    internal static class TestReflection
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        /// <summary>
        /// コンストラクタを呼ばずにインスタンスを作る
        /// </summary>
        /// <remarks>
        /// 画面のクラス(MainForm・TimingPropertyForm)はコンストラクタで osu! の監視スレッドなどを動かすため、
        /// テストではコンストラクタを通さずに作り、必要なフィールドだけを設定する。
        /// 初期化していないオブジェクトの後片付け(ファイナライザ)で落ちないよう、ファイナライザは止める
        /// </remarks>
        internal static T CreateWithoutConstructor<T>()
        {
            var instance = (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
            GC.SuppressFinalize(instance!);
            return instance;
        }

        internal static object? GetField(object target, string name) =>
            FindField(target.GetType(), name).GetValue(target);

        internal static void SetField(object target, string name, object? value) =>
            FindField(target.GetType(), name).SetValue(target, value);

        /// <summary>
        /// private のメソッドを呼ぶ
        /// </summary>
        internal static object? Invoke(object target, string name, params object?[] args)
        {
            var method = target.GetType().GetMethod(name, Flags) ?? throw new MissingMethodException(target.GetType().Name, name);
            try
            {
                return method.Invoke(target, args);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// private の static メソッドを呼ぶ
        /// </summary>
        internal static object? InvokeStatic(Type type, string name, params object?[] args)
        {
            var method = type.GetMethod(name, Flags) ?? throw new MissingMethodException(type.Name, name);
            try
            {
                return method.Invoke(null, args);
            }
            catch (TargetInvocationException ex) when (ex.InnerException != null)
            {
                throw ex.InnerException;
            }
        }

        private static FieldInfo FindField(Type type, string name)
        {
            for (Type? t = type; t != null; t = t.BaseType)
            {
                var field = t.GetField(name, Flags);
                if (field != null) return field;
            }
            throw new MissingFieldException(type.Name, name);
        }

        /// <summary>
        /// STA のスレッドで処理を実行する (WinForms のコントロールを作るテスト用)
        /// </summary>
        internal static void RunInSta(Action action)
        {
            Exception? error = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (error != null) throw new AggregateException(error);
        }
    }
}
