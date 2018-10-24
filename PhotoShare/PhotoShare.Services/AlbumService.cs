﻿using AutoMapper.QueryableExtensions;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Models.Enums;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class AlbumService : IAlbumService
    {

        private readonly PhotoShareContext context;

        public AlbumService(PhotoShareContext context)
        {
            this.context = context;
        }
        public TModel ById<TModel>(int id) => this.By<TModel>(x => x.Id == id).SingleOrDefault();


        public TModel ByName<TModel>(string name) => this.By<TModel>(x => x.Name == name).SingleOrDefault();

        public Album Create(int userId, string AlbumTitle, string bgColor, string[] tags)
        {
            var colorAsEnum = Enum.Parse<Color>(bgColor, true);
          
            var album = new Album()
            {
                Name = AlbumTitle,
                BackgroundColor = colorAsEnum,
            };

            context.Albums.Add(album);
            context.SaveChanges();

            var albumRole = new AlbumRole
            {            
                UserId = userId,
                Album = album,
            };

            context.AlbumRoles.Add(albumRole);
            context.SaveChanges();

            foreach (var tag in tags)
            {
                var currentTagId = this.context.Tags.FirstOrDefault(x => x.Name == tag).Id;

                var albumTag = new AlbumTag
                {
                    Album = album,
                    TagId = currentTagId,
                };
                this.context.AlbumTags.Add(albumTag);
            }
            context.SaveChanges();

            return album;
        }

        public bool Exists(int id) => this.ById<Album>(id) != null;

        public bool Exists(string name) => this.ByName<Album>(name) != null;

        private IEnumerable<TModel> By<TModel>(Func<Album, bool> predicate) =>
            this.context.Albums
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();
    }
}
