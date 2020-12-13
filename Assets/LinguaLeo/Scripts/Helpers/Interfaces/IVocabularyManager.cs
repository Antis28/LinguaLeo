using System.Collections.Generic;

namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    // TODO: Переименовать класс, так как он позволяет загружать не только словарь.
    public interface IVocabularyManager<T> where T: class
    {
        T Load();
        void Save(T vocabulary);
    }
}
