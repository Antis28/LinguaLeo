using System.Collections.Generic;

namespace LinguaLeo.Scripts.Helpers.Interfaces
{
    public interface IVocabularyManager
    {
        WordCollection Load();
        void Save(WordCollection vocabulary);
    }
}
