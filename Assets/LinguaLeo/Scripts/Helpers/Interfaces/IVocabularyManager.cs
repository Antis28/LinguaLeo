using System.Collections.Generic;

namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    public interface IVocabularyManager<T> where T: class
    {
        T Load();
        void Save(T vocabulary);
    }
}
