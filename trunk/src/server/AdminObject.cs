//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Exceptions;
using Boxerp.Models;
using Castle.ActiveRecord;
using NHibernate;

namespace Boxerp.Objects
{

public class AdminObject : MarshalByRefObject, IAdmin
{
    private static SessionsManager sessionsMgr = SessionsManager.GetInstance();

    // Constructor
    public AdminObject ()
    {}

    public User[] GetUsers()
    {
        try
        {
            Console.WriteLine("user session=" + UserInformation.GetSessionToken());
            if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
            {
                Console.WriteLine("valid user");
                return User.FindAll();
            }
            else
            {
                Console.WriteLine("not valid user");
                return null;
            }
        }
        catch (UnauthorizedException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
            return null;
        }
    }

    public Group[] GetGroups()
    {
        try
        {
            if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
            {
                return Group.FindAll();
            }
            else
            {
                return null;
            }
        }
        catch (UnauthorizedException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
            return null;
        }
    }

    public Group[] GetDistinctGroups(User u)
    {
        try
        {
            if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
            {
                if (u != null)
                {
                    Group[] allGroups = Group.FindAll();
                    ArrayList groups = new ArrayList();
                    foreach(Group i in allGroups)
                    {
                        if (!u.Groups.Contains(i))
                            groups.Add(i);
                    }
                    if (groups.Count > 0)
                        return (Group[])groups.ToArray(groups[0].GetType());
                    else
                        return null;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        catch (UnauthorizedException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
            return null;
        }
    }

    public Enterprise[] GetEnterprises()
    {
        try
        {
            if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
            {
                return Enterprise.FindAll();
            }
            else
            {
                return null;
            }
        }
        catch (UnauthorizedException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
            return null;
        }
    }

    public User GetUser(string username, string password)
    {
        /*Console.WriteLine("User information=" + UserInformation.GetUser());
        Console.WriteLine("Haciendo login= " + username +"," + password);
              User u = User.FindByUsernameAndPasswd(username, password);
        return u;	*/
        return null;
    }

    public int SaveUser(User user)
    {
        try
        {
            if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
            {
                //Console.WriteLine(u.Id +","+u.UserName+","+u.RealName+","+
                //                  u.Email +","+u.Active);
                //ActiveRecordBase.Replicate(u);
                User persistentUser;
                if (user.Id != 0)
                {
                    persistentUser = User.Find(user.Id);
                    persistentUser.UserName = user.UserName;
                    persistentUser.RealName = user.RealName;
                    persistentUser.Email = user.Email;
                    persistentUser.Active = user.Active;
                    persistentUser.Password = user.Password;
                    persistentUser.Save();
                }
                else
                {
                    persistentUser = user;
                    persistentUser.Save();
                }
                    foreach (Group i in user.Groups)
                    {
                        Group group = Group.Find(i.Id);
                        if (group != null)
                        {
                            if (! group.Users.Contains(persistentUser))
                            {

                            Console.WriteLine(persistentUser.Id +","+ persistentUser.UserName+","+ persistentUser.RealName);
                            group.Users.Add(persistentUser);
                            //persistentUser.Groups.Add(g);
                            //g.Update();
                        }
                        group.Save();
                    }
                    Console.WriteLine("Group id=" + i.Id);
                }
                persistentUser.Save();
                return persistentUser.Id;
            }
            else
            {
                Console.WriteLine("not valid user");
                return -1 ;
            }
        }
        catch (UnauthorizedException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
            if (ex.InnerException != null)
                Console.WriteLine("ERROR:" + ex.InnerException.Message +
                                  ":" + ex.InnerException.StackTrace);
            throw ex;
        }
    }

    public int DeleteUser(User user)
    {
        try
        {
            if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
            {
                //Console.WriteLine(u.Id +","+u.UserName+","+u.RealName+","+
                //                  u.Email +","+u.Active);
                //ActiveRecordBase.Replicate(u);
                User persistentUser;
                if ((user == null) || (user.Id != 0))
                {
                    persistentUser = User.Find(user.Id);
                }
                else
                {
                    throw new NullReferenceException("User not found");
                }
                int id = persistentUser.Id;
                foreach (Group i in user.Groups)
                {
                    Group group = Group.Find(i.Id);
                    if (group != null)
                    {
                        if (group.Users.Contains(persistentUser))
                        {
                            group.Users.Remove(persistentUser);
                        }
                        group.Save();
                    }
                }
                persistentUser.Delete();
                return id;
            }
            else
            {
                Console.WriteLine("not valid user");
                return -1 ;
            }
        }
        catch (UnauthorizedException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
            if (ex.InnerException != null)
                Console.WriteLine("ERROR:" + ex.InnerException.Message +
                                  ":" + ex.InnerException.StackTrace);
            throw ex;
        }
    }

}
}
