using RovioTest.Models;
using UnityEngine;

namespace RovioTest.View
{
    public class CourtView : MonoBehaviour
    {
        private CourtModel _model;
        private Rigidbody _rigidBody;

        public void SetModel(CourtModel model)
        {
            _model = model;
        }
    }
}