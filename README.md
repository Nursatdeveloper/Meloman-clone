# Meloman-clone

This application was build only for the purpose of practice!

My application uses ASP.NET Core 5 for backend, and JS/jQuery for frontend.
For database, I used PostgreSql which is hosted in Heroku platform.

I implemented only Books category, because other product types use the same functionality and take unnecessary memory on heroku's limited database.

Application has a number of functionalities:
1) Authentication and Authorization |
  This functionality implements cookie-based authorization, claim-based authorization, and policy-based authorization.
2) Sorting |
  Meloman-clone sorts books based on the price, category, genre, author, book cover, etc.
3) Admin profile |
  The user, who has admin credentials, can access to admin portal, where user can execute CRUD operations and get list of books, reviews, and orders.
4) Export to Excel |
  Admin can export list of books, reviews and orders to excel file.
5) Give review to particular book (requires authentication) |
6) Add to basket of orders |
7) Make an order (requires authentication) |
  This functionality is implemented in /Books/Details/{id} page and User/Profile. The only difference is than in Details page the User can make order for only that particular book, whereas in Profile page the User can increase/decrease amount of books and make order for different type of books.
8) Saving Book photos in binary |
  Book photos are saved in binary format to database. 
9) "Download in PDF" service for users

Link: http://meloman-clone-db.herokuapp.com
