# ProductManagement
This project uses docker to run the .net api and a MySQL database

## Getting Started

### Requirements
* [Docker & Docker-compose](https://docs.docker.com/compose/install/) 
### Set-up
1. Clone the repository
2. On command line navigate to the cloned repository
3. Run docker-compose to launch the api and the MySQL database
   ```sh
   docker-compose up -d 
   ```
4. Run the sql script to create a database and fill it with sample data (one time only)
   ```sh
   docker exec -i mysqldb mysql -u user -ppassword < seed_database.sql
   ```
5. Project should now be running and accessible
 ### Usage
 * GET requests
  1. [http://localhost:8000/api/products](http://localhost:8000/api/products) Gets all the products
  2. [http://localhost:8000/api/products/{id}](http://localhost:8000/api/products/1) Gets a certain product by its id
  3. [http://localhost:8000/api/group/tree](http://localhost:8000/api/group/tree) Get the categories in a tree view
  4. [http://localhost:8000/api/shop/{id}/products](http://localhost:8000/api/shop/1/products) Get all the products present in a shop by the shop ID
 * POST
  1. [http://localhost:8000/api/products](http://localhost:8000/api/products) Insert a new product into the database. Post body should be in json format as shown below
  ```json
  {
    "name": "example",
    "groupid": 1,
    "price": 5,
    "vatprice": 0,
    "vatpercentage": 20,
    "shopid": [1]
  }
  ```
 ### Finishing up
 * To shutdown the running container:
    ```sh
    docker-compose down
    ```
