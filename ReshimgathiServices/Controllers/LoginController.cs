﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReshimgathiServices.Business;
using ReshimgathiServices.Requests;
using ReshimgathiServices.Responses;
using Swashbuckle.Swagger.Annotations;

namespace ReshimgathiServices.Controllers
{
    /// <summary>
    /// This controller helps to manage all types of Login operations.
    /// </summary>
    [RoutePrefix("api/login")]
    [CatchException]
    public class LoginController : ApiController
    {
        /// <summary>
        /// Verify Username and password.
        /// </summary>
        /// <returns></returns>
        [Route("verify")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "Verify Login Details.", typeof(Response<LoginResponse>))]
        public HttpResponseMessage VerifyLoginDetails(LoginRequest req)
        {
            Response<LoginResponse> lor = new Response<LoginResponse>();
            bool isValidCreds = false;
            try
            {
                LoginOperations loginOp = new LoginOperations();
                var loginDetails = loginOp.VerifyLoginCreds(req.Username, req.Password);

                if(loginDetails != null)
                {
                    isValidCreds = true;
                }
                
                if (isValidCreds)
                {
                    UserProfileOperations uop = new UserProfileOperations();
                    UserProfilePicturesOperations uppo = new UserProfilePicturesOperations();
                    lor.Message = "User found. Welcome to Reshimgathi Matrimony !!";
                    lor.ResponseObj = new LoginResponse()
                    {
                        LoginStatus = true,
                        Token = TokenManager.GenerateToken(req.Username),
                        UserProfileId = loginDetails.UserProfileId,
                        //UserProfileDetails = uop.GetUserProfileDetails(loginDetails.UserProfileId),
                        //UserProfilePictures = uppo.GetUserProfilePictures(loginDetails.UserProfileId)
                    };
                } 
                else
                {
                    lor.Message = "User Not Found !! Please try with correct login Creds.";
                    lor.ResponseObj = new LoginResponse()
                    {
                        LoginStatus = false,
                        Token = null,
                        UserProfileId = Guid.Empty,
                    };
                } 

                lor.AdditionalMessage = "Additional note found here.";
                lor.HttpStatus = HttpStatusCode.OK.ToString();
                lor.Success = true;
            }
            catch(Exception e)
            {
                lor.Success = false;
                lor.Message = "Internal Server error. Please contact admin or try after some time.";
                lor.AdditionalMessage = e.Message;
                lor.HttpStatus = HttpStatusCode.InternalServerError.ToString();
                lor.ResponseObj = new LoginResponse()
                {
                    LoginStatus = false,
                    Token = null,
                    UserProfileId = Guid.Empty,
                };
            }

            return Request.CreateResponse(HttpStatusCode.OK, lor);
        }

        [Route("changepassword")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "change My Password.", typeof(Response<ChangePasswordResponse>))]
        public HttpResponseMessage ChangePassword(ChangePassword request)
        {
            Response<ChangePasswordResponse> lor = new Response<ChangePasswordResponse>();
            bool isPasswordUpdated = false;
            try
            {
                LoginOperations loginOp = new LoginOperations();
                bool isCredsVerified = loginOp.VerifyPasswordWithProfileId(request);

                if (isCredsVerified)
                {
                    isPasswordUpdated = loginOp.ChangePassword(request);
                    lor.Message = "New Password is updated successfully.";
                    lor.ResponseObj = new ChangePasswordResponse()
                    {
                        Result = isPasswordUpdated
                    };
                }
                else
                {
                    lor.Message = "User Not Found !! Please try with correct login Creds.";
                    lor.ResponseObj = new ChangePasswordResponse()
                    {
                        Result = isPasswordUpdated
                    };
                }

                lor.AdditionalMessage = "Additional note found here.";
                lor.HttpStatus = HttpStatusCode.OK.ToString();
                lor.Success = true;
            }
            catch (Exception e)
            {
                lor.Success = false;
                lor.Message = "Internal Server error. Please contact admin or try after some time.";
                lor.AdditionalMessage = e.Message;
                lor.HttpStatus = HttpStatusCode.InternalServerError.ToString();
                lor.ResponseObj = new ChangePasswordResponse()
                {
                    Result = isPasswordUpdated
                };
            }

            return Request.CreateResponse(HttpStatusCode.OK, lor);
        }
        

        [Route("getprofiles")]
        [HttpGet]
        [RAuthFilter]
        [SwaggerResponse(HttpStatusCode.OK, "Get All Profiles.", typeof(string))]
        public HttpResponseMessage GetProfiles()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }
    }
}
