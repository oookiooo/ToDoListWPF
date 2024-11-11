using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWPF.ViewModels;

namespace NUnitTest
{
    [TestFixture]
    public class TaskDialogViewModelTests
    {
        private TaskDialogViewModel _viewModel;
        private bool _closeCalled;
        private bool _dialogResult;

        [SetUp]
        public void Setup()
        {
            _viewModel = new TaskDialogViewModel();
            _viewModel.Close += (s, result) => {
                _closeCalled = true;
                _dialogResult = result;
            };
        }

        [Test]
        public void SaveCommand_SetsCloseWithTrue()
        {
            // Act
            _viewModel.SaveCommand.Execute(null);

            // Assert
            Assert.IsTrue(_closeCalled);
            Assert.IsTrue(_dialogResult);
        }

        [Test]
        public void CancelCommand_SetsCloseWithFalse()
        {
            // Act
            _viewModel.CancelCommand.Execute(null);

            // Assert
            Assert.IsTrue(_closeCalled);
            Assert.IsFalse(_dialogResult);
        }

        [Test]
        public void SettingProperties_RaisesPropertyChanged()
        {
            // Arrange
            bool titleChanged = false;
            _viewModel.PropertyChanged += (s, e) => {
                if (e.PropertyName == "Title")
                    titleChanged = true;
            };

            // Act
            _viewModel.Title = "New Title";

            // Assert
            Assert.IsTrue(titleChanged);
        }
    }
}
