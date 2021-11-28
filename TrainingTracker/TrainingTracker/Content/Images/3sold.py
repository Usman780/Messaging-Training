import urllib2


class Item:
    title = ''
    id = ''
    price = ''
    image = ''
    seller = ''
    sellerUrl = ''
    url = ''
    sold = 0
    soldOnpage = ""

    def isContain(self, itemTite, searchRtile):

         searchRtileArr = searchRtile.split(" ")
         if len(searchRtileArr) < 3:
          return 1

         count = 0;
         for x in searchRtileArr:
          if x.lower() in itemTite.lower():
            count = count + 1

         if (count / len(searchRtileArr)) > 0.50:
             return 1
         return 0


class Text:
    def add(self, path):

        n = Item()
        page = urllib2.urlopen(path)
        from bs4 import BeautifulSoup
        soup = BeautifulSoup(page, 'html.parser')

        try:
            n.price = soup.find("span", {"id": "mm-saleDscPrc"}).text

        except:
            n.price = soup.find(attrs={'itemprop': 'price'})['content'] + ' ' + \
                      soup.find(attrs={'itemprop': 'priceCurrency'})['content']

        n.title = soup.title.string
        n.image = soup.find(attrs={'itemprop': 'image'})['src']

        n.sellerUrl = soup.find("a", {"id": "mbgLink"})['href']
        n.seller = soup.find("a", {"id": "mbgLink"}).text
        return n

    def hdd(self, path, country, keywords):
	    highest = Item()
            url = path;
            opener = urllib2.build_opener()

            opener.addheaders = [('User-agent', 'Mozilla/5.0')]

            from bs4 import BeautifulSoup
            soup = BeautifulSoup(opener.open(url).read(), 'html.parser')
            number = ""
            count = 0
            count2 = 0
            errorText = ""
            itemUrl = ''
            te="hello"

            for li in soup.findAll('div', {"class": "g"}):
                try:
                    text = li.find('span', {"class", "st"})
                    for p in li.findAll(attrs={'class': 'r'}):
                        if itemUrl is '':
                            if p is not None:
                                p = p.find('a')["href"].split('=')[1]
                                if "itm" in p:
                                    itemUrl = p
                    my_list = list()
                    if text is not None:
                        text = text.text
                        errorText = "first"
                        if "Results" in text:
                            url = "http://webcache.googleusercontent.com/search?q=cache:" + \
                                  li.find('h3', {"class": "r"}).find("a")["href"].split("q=")[1]

                            opener = urllib2.build_opener()
                            opener.addheaders = [('User-agent', 'Mozilla/5.0')]
                            from bs4 import BeautifulSoup
                            soup = BeautifulSoup(opener.open(url).read(), 'html.parser')
                            errorText = "2nd"
                            te=soup.prettify();
                            for pr in soup.find_all("div", {"class", "s-item__wrapper clearfix"}):
                                item = Item()
                                count = count + 1

                                try:
                                    item.sold = pr.find("span", {"class", "NEGATIVE"}).text
                                    count2 = count2 + 1
                                    if "sold" in item.sold:
                                        item.sold = int(item.sold.split(" ")[0].replace(",", ""))
                                        item.title = pr.find("h3", {"class", "s-item__title"}).text
                                        item.url = pr.find("a", {"class", "s-item__link"})["href"]
                                        item.price = pr.find("span", {"class", "s-item__price"}).text

                                        my_list.append(item)
                                        errorText = "third"

                                except Exception as e:
                                    number = number
                        if (len(my_list) > 0):
                            errorText = "4th"
                            my_list.sort(key=lambda x: x.sold, reverse=True)
                            opener = urllib2.build_opener()
                            opener.addheaders = [('User-agent', 'Mozilla/5.0')]
                            errorText = "5th"
                            newUrl = my_list[0].url
                            if country not in newUrl:
                                array = newUrl.split("/")
                                newUrl = "http://" + country
                                i = 2
                                while len(array) > (i + 1):
                                    i = i + 1
                                    newUrl = newUrl + "/" + array[i]
                            errorText = "6th"
                            from bs4 import BeautifulSoup
                            soup = BeautifulSoup(opener.open(newUrl).read(), 'html.parser')
                            img = soup.find(attrs={'itemprop': 'image'})
                            if img is not None:
                                img = img['src']

                            my_list[0].image = img
                            temp = soup.find(attrs={'class': 'vi-qtyS-hot-red'})
                            tempPrice = soup.find(attrs={'id': 'convbinPrice'})
                            if tempPrice is not None:
                                my_list[0].price = tempPrice.text
                            if temp is not None:
                                my_list[0].soldOnpage = temp.find("a").text
                                my_list[0].soldOnpage = int(my_list[0].soldOnpage.split(" ")[0].replace(",", ""))
                                if my_list[0].soldOnpage < my_list[0].sold:
                                    my_list[0].soldOnpage = my_list[0].sold
                            else:
                                my_list[0].soldOnpage = my_list[0].sold
                            return my_list[0]





                except Exception as e:
                    if errorText is "6th":
                        return my_list[0]
                        return errorText
            
            try:

                opener = urllib2.build_opener()
                opener.addheaders = [('User-agent', 'Mozilla/5.0')]
                errorText = "7th"
                from bs4 import BeautifulSoup
                soup = BeautifulSoup(opener.open(itemUrl).read(), 'html.parser')
                img = soup.find(attrs={'itemprop': 'image'})['src']
                if img is not None:
                    highest.image = soup.find(attrs={'itemprop': 'image'})['src']
                price = soup.find(attrs={'class': 'vi-qtyS-hot-red'})
                if price is not None:
                    highest.soldOnpage = soup.find(attrs={'class': 'vi-qtyS-hot-red'}).find("a").text
                    return highest
            except Exception as e:
                return None



