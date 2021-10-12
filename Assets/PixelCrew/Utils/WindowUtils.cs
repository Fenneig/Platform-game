﻿using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        private static GameObject _window;
        private static Canvas _canvas;
        
        private static void CreateWindow()
        {
            Object.Instantiate(_window, _canvas.transform);
        }

        public static void CreateWindow(string resourcePath)
        {
            _window = Resources.Load<GameObject>(resourcePath);
            _canvas = Object.FindObjectOfType<Canvas>();

            var canvases = Object.FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (canvas.name == "Canvas") _canvas = canvas;
            }
            
            CreateWindow();
        }
    }
}