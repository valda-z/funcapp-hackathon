# Single Page Application hosted in Function App with REST APIs

This experiment is based on previous experiment https://github.com/valda-z/funcapp-hackathon/blob/master/FunctionApp_REST/README.md .

Now we will create second REST API function which will return all ToDos from document database. And also we will configure proxy rules which will redirect user requests for static assets(HTML pages, scripts, etc) to blob storage and for API calls to Function App itself. 

### Step 1: Create new function

In Azure portal select your Function App. In the left pane of control plane screen select "plus" symbol just on right side of "Function" section. Click on "+" button.
In list of offered template filter for "JavaScript" templates and select "HttpTrigger - JavaScript" function template. Enter name `ToDoList` and hit button "Create".

![img0.png](img/img1.png "")

Go to "Integrate" option menu in new function `ToDoList`. Configure there New Input

![img0.png](img/img2.png "")

From list of possible input templates select Azure CosmosDB and hit button "Select"

![img0.png](img/img3.png "")

In Azure Cosmos DB configuration screen define connection to Cosmos DB (same like in case of function `ToDoAdd`
* Database name - `outDatabase`
* Collection Name - `MyCollection`
* Select valid connection to Azure Cosmos DB account connection

![img0.png](img/img4.png "")

Copy/Paste this function code to `ToDoList` function body:

```javascript
module.exports = function(context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    context.res = {
        // status: 200, /* Defaults to 200 */
        body: context.bindings.inputDocument
    };

    context.done();
};
```

### Step 2: Define proxy rules for request routing

#### Routing rule for API calls

Go to configuration page for function `ToDoList` and select menu item "Manage" and click on link "Copy" in section "Host Keys (All functions) for "default" key. Save API key value for future use.

![img0.png](img/img6.png "")

Click on "+" button in "Proxies (preview)" section, in Proxy definition page use these values:
* Name: `API`
* Route template: `/api/{*rest}`
* Backend URL `https://%WEBSITE_HOSTNAME%/api/{rest}?code=[YOUR API KEY]`

And hit button "Create" to save changes.

![img0.png](img/img5.png "")

#### Routing rule for default document

Click on "+" button in "Proxies (preview)" section, in Proxy definition page use these values:
* Name: `WebServerDefaults`
* Route template: `/`
* Backend URL `https://valda.blob.core.windows.net/pub/www-funcapp/index.html`

And hit button "Create" to save changes.

#### Routing rule for static content

Click on "+" button in "Proxies (preview)" section, in Proxy definition page use these values:
* Name: `WebServer`
* Route template: `{*path}`
* Backend URL `https://valda.blob.core.windows.net/pub/www-funcapp/{path}`

And hit button "Create" to save changes.


