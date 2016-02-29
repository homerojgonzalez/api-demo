﻿using System;
using System.Web;
using System.Web.UI;

namespace AspNetClientEncryptionExample
{
	
	public partial class TestJson : System.Web.UI.Page
	{
		public void BtnTestClicked(object sender, EventArgs args)
		{
			if(this.IsPostBack) 
			{
				OAuthTokenGenerator tokenGenerator = new OAuthTokenGenerator ();
				VerifyOAuthToken(tokenGenerator.GetToken ());
			}
		}

		protected void VerifyOAuthToken(OAuthToken oAuthResult) 
		{

			if(oAuthResult.errorflag == false)
			{
				// In case of not using any OAuth2.0 Library
				// Use following when OAuth2.0 is caseinsesitive at Paytrace. 
				// string OAuth = String.Format ("{0} {1}", OAuthResult.token_type, OAuthResult.access_token);
				// For now OAuth2.0  is not caseinsesitive at PayTrace - ESC-141 so use 'Bearer'
				string OAuth = String.Format ("Bearer {0}", oAuthResult.access_token);

				// Build a Transaction 
				BuildTransaction (OAuth);

			} 
			else // Error for OAuth
			{
				// Do you code here to handle the OAuth error

				// Optional - Display the OAuth Error 
				DisplayOAuthError(oAuthResult);
			}
		}

		public void BuildTransaction(string oAuth)
		{
			// Vault Sale by Customer Id Request
			VaultSaleByCustomerIdRequest  requestVaultSaleByCustomerId = new VaultSaleByCustomerIdRequest ();

			// for Vault Sale by Customer Id Transaction Request execuation 
			TestJsonGenerator  vaultSaleByCustomerIDGenerator  = new  TestJsonGenerator();

			// Assign the values to the void Transaction Request.
			requestVaultSaleByCustomerId = BuildRequestFromFields(requestVaultSaleByCustomerId);

			// To make Void Transaction Request and store the response
			var result = vaultSaleByCustomerIDGenerator.TestVaultSaleByCustomerIdTrans(oAuth,requestVaultSaleByCustomerId);

			//display the void Transaction Response
			WriteResults(result);
		}

		public void DisplayOAuthError(OAuthToken OAuthResult)
		{
			// Optional - Display the OAuth Error 
			//System.Web.HttpContext.Current.Response.Write ("<br>" + "Http Status Code & Description : " +  OAuthResult.Error.token_error_http  + "<br>");
			Response.Write (" Http Status Code & Description : " +  OAuthResult.Error.token_error_http  + "<br>");
			Response.Write (" API Error : " +  OAuthResult.Error.error + "<br>");
			Response.Write (" API Error Message : " +  OAuthResult.Error.error_description+ "<br>");
			Response.Write (" Token Request: " + "Failed!" + "<br>");

		}


		protected VaultSaleByCustomerIdRequest BuildRequestFromFields(VaultSaleByCustomerIdRequest requestVaultSaleByCustomerId)
		{
			// Build the Vault Sale by customerId fields from the input sources.
			// Customer ID can be obtained from any sources where it is stored previously. 
			// Strorage source could be at the PayTrace repository(if used create customer profile earlier) or at the client repository.  

			requestVaultSaleByCustomerId.amount = 0.501;
			requestVaultSaleByCustomerId.customer_id = "customerTest12";

			return requestVaultSaleByCustomerId;

		}

		protected void WriteResults( PayTraceBasicSaleResponse result) 
		{

			if(null != result.ErrorMsg  && result.success == false )
			{
				Response.Write ( "<br>" + "Http Error Code & Error : " + result.ErrorMsg + "<br>");

				Response.Write ("Success : " + result.success + "<br>"); 
				Response.Write ("response_code : " + result.response_code + "<br>");   
				Response.Write ("status_message : " + result.status_message + "<br>"); 
				Response.Write ("external_transaction_id : " + result.external_transaction_id + "<br>"); 
				//Response.Write ("masked_card_number : " + result.masked_card_number + "<br>"); 

				//Check the actual API errors with appropriate code
				Response.Write (" API errors : "+ "<br>");
				foreach (var item in result.errors) 
				{	
					// to read Error message with each error code in array.
					foreach (var errorMessage in (string[])item.Value) 
					{
						Response.Write (item.Key  + "=" + errorMessage + "<BR>");
					}
				}
				Response.Write ("Keyed sale: " + "Failed!" + "<br>");	

			} 
			else
			{
				// Do your code when Response is available based on the response_code. 
				// Please refer PayTrace-HTTP Status and Error Codes page for possible errors and Response Codes
				// For transation successfully approved 
				if (result.response_code == 101 && result.success == true ) 
				{
					// Do you code for any additional verification

					// Display Response - optional
					DisplaySaleResponse(result);
					Response.Write ("sale: " + "Success!" + "<br>");		

				}

				else
				{
					// Do your code here based on the response_code - use the PayTrace http status and error page for reference
					// Do your code for any additional verification - avs_response and csc_response

					//Display Response
					DisplaySaleResponse(result);
					Response.Write ("Error : " + result.ErrorMsg + "<br>");

				}

				// Do your code for Any additional task !
			}
		}

		//Display the Keyed Sale Response
		protected void DisplaySaleResponse(PayTraceBasicSaleResponse result)
		{
			Response.Write ( "<br>" + "Success : " + result.success + "<br>"); 
			Response.Write ("response_code : " + result.response_code + "<br>");   
			Response.Write ("status_message : " + result.status_message + "<br>"); 
			Response.Write ("transaction_id : " + result.transaction_id + "<br>"); 
			Response.Write ("approval_code : " + result.approval_code + "<br>"); 
			Response.Write ("approval_message : " + result.approval_message + "<br>"); 
			Response.Write ("avs_response : " + result.avs_response + "<br>"); 
			Response.Write ("csc_response : " + result.csc_response + "<br>"); 
			Response.Write ("external_transaction_id : " + result.external_transaction_id + "<br>"); 
			//Response.Write ("masked_card_number : " + result.masked_card_number + "<br>"); 
		}


	}


}

