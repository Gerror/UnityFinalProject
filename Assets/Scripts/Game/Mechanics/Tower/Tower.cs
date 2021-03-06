using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Game.Core;
using Game.Mechanics.Mob;
using UnityEngine;
using Zenject;

namespace Game.Mechanics.Tower
{
    public class Tower : MonoBehaviour
    {
        private TowerOwner _towerOwner;
        private int _towerIndex;
        private int _fieldIndex;
        
        // fieldIndex, secondFieldIndex, TowerIndex, currentMergeLevel
        public event Action<int, int, int, int> MergeEvent;

        private TowerLevels _towerLevels;
        private GameSettings _gameSettings;
        private MobSpawnMechanics _mobSpawnMechanics;
        private ManaMechanics _manaMechanics;

        public event Action InitEvent;
        public TowerOwner TowerOwner => _towerOwner;
        public ManaMechanics ManaMechanics => _manaMechanics;
        public MobSpawnMechanics MobSpawnMechanics => _mobSpawnMechanics;
        public TowerLevels TowerLevels => _towerLevels;

        public int TowerIndex => _towerIndex;
        public int FieldIndex => _fieldIndex;
        
        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void Init(TowerOwner towerOwner, MobSpawnMechanics mobSpawnMechanics,
            ManaMechanics manaMechanics, int mergeLevel, int fieldIndex, int towerIndex)
        {
            _towerLevels = GetComponent<TowerLevels>();

            _mobSpawnMechanics = mobSpawnMechanics;
            _manaMechanics = manaMechanics;
            _towerOwner = towerOwner;
            _fieldIndex = fieldIndex;
            _towerIndex = towerIndex;
            
            _towerLevels.SetCurrentLevel(LevelType.BaseLevel, towerOwner.BaseLevels[TowerIndex]);
            _towerLevels.SetCurrentLevel(LevelType.MergeLevel, mergeLevel);
            InitEvent?.Invoke();
        }

        public void Merge(Tower _secondTower)
        {
            if (!CanMerge(_secondTower))
                return;
            
            MergeEvent?.Invoke(FieldIndex, _secondTower.FieldIndex, TowerIndex, _towerLevels.Levels[(int) LevelType.MergeLevel]);
        }

        public bool CanMerge(Tower _secondTower)
        {
            if (_secondTower == this)
                return false;
            
            if (_secondTower._towerLevels.Levels[(int) LevelType.MergeLevel] !=
                _towerLevels.Levels[(int) LevelType.MergeLevel])
                return false;

            if (_towerLevels.Levels[(int) LevelType.MergeLevel] ==
                _gameSettings.MaxLevels[(int) LevelType.MergeLevel] - 1)
                return false;

            return true;
        }
    }
}