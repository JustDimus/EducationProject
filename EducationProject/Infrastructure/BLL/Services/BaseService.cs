﻿using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using Infrastructure.DAL.EF.Mappings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Services
{
    public abstract class BaseService<TEntity, TOut> : IBusinessService<TOut> where TEntity : class
    {
        protected BaseRepository<TEntity> entity;

        protected AuthorizationService authService;

        public BaseService(BaseRepository<TEntity> baseEntityRepository,
            AuthorizationService authorisztionService)
        {
            entity = baseEntityRepository;

            authService = authorisztionService;
        }

        protected virtual bool createCheckAuth { get => true; }

        protected virtual bool updateCheckAuth { get => true; }

        protected virtual bool deleteCheckAuth { get => true; }

        protected virtual int defaultGetCount { get => 30; }

        protected virtual Expression<Func<TEntity, bool>> defaultGetCondition { get => e => true; }

        protected abstract bool ValidateEntity(TOut entity);

        protected abstract Expression<Func<TEntity, TOut>> FromBOMapping { get; }

        protected abstract TEntity Map(TOut entity);

        protected abstract Expression<Func<TEntity, bool>> IsExistExpression(TOut entity);

        public bool Create(ChangeEntityDTO<TOut> createEntity)
        {
            if (this.createCheckAuth == true)
            {
                if (this.authService.AuthenticateAccount(createEntity.Token) == 0)
                {
                    return false;
                }
            }

            if (ValidateEntity(createEntity.Entity) == false)
            {
                return false;
            }

            entity.Create(Map(createEntity.Entity));

            entity.Save();

            return true;
        }

        public bool Delete(ChangeEntityDTO<TOut> deleteEntity)
        {
            if (deleteCheckAuth == true)
            {
                if (authService.AuthenticateAccount(deleteEntity.Token) == 0)
                {
                    return false;
                }
            }

            entity.Delete(Map(deleteEntity.Entity));

            entity.Save();

            return true;
        }

        public IEnumerable<TOut> Get(PageInfoDTO pageInfo)
        {
            if (ValidatePageInfo(pageInfo) == false)
            {
                return null;
            }

            return entity.GetPage<TOut>(defaultGetCondition, FromBOMapping, pageInfo.PageNumber, pageInfo.PageSize);
        }

        public bool IsExist(TOut entity)
        {
            return this.IsExist(IsExistExpression(entity));
        }

        public bool IsExist(Expression<Func<TEntity, bool>> condition)
        {
            return entity.Any(condition);
        }

        public string LogIn(string login, string password)
        {
            return authService.AuthorizeAccount(login, password);
        }

        public bool LogOut(string token)
        {
            return authService.DeauthorizeAccount(token);
        }

        public bool Update(ChangeEntityDTO<TOut> updateEntity)
        {
            if (updateCheckAuth)
            {
                if (Authenticate(updateEntity.Token) == 0)
                {
                    return false;
                }
            }

            if (ValidateEntity(updateEntity.Entity) == false)
            {
                return false;
            }

            entity.Update(Map(updateEntity.Entity));

            entity.Save();

            return true;
        }

        protected int Authenticate(string token)
        {
            return authService.AuthenticateAccount(token);
        }

        protected bool ValidatePageInfo(PageInfoDTO pageInfo)
        {
            return !(pageInfo.PageNumber < 0 || pageInfo.PageSize <= 0);
        }
    }
}
