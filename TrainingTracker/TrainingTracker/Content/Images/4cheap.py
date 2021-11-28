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


class Text:
    def add(self, path):
        n = Item()
        iflistempty = ''
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

    def hdd(self, path):
        dat="first step"
        url = path;
        opener = urllib2.build_opener()
        opener.addheaders = [('User-agent', 'Mozilla/5.0')]
        dat="second step"
        from bs4 import BeautifulSoup
        soup = BeautifulSoup(opener.open(url).read(), 'html.parser')
        number = ""
        count = 0
        count2 = 0
        dat="third step"
        for li in soup.findAll('div', {"class": "g"}):

            try:
                iflistempty = ""
                highest = ""
                dat = "4th step"
                text = li.find('span', {"class", "st"})
                text1 = li.find('div', {"class": "s"})
                if text1 is not None :
                    text1=text1.find('div', {"class", "kv"})
                    if text1 is not None:
                        text1=text1.find('cite').text
                dat = text1
                if text1 is not None:
                    if ".." in text1:
                        if iflistempty == '':
                            iflistempty = text1

                    elif "itm" in text1:
                        if iflistempty == '':
                            iflistempty = text1
                my_list = list()
                if text is not None:
                 text=text.text
                 if "Results" in text:
                    url = "http://webcache.googleusercontent.com/search?q=cache:" + \
                          li.find('h3', {"class": "r"}).find("a")["href"].split("q=")[1]
                    dat = "5th step"
                    opener = urllib2.build_opener()
                    opener.addheaders = [('User-agent', 'Mozilla/5.0')]
                    from bs4 import BeautifulSoup
                    soup = BeautifulSoup(opener.open(url).read(), 'html.parser')
                    dat = "6th step"
                    for pr in soup.find_all("div", {"class", "s-item__wrapper clearfix"}):
                        item = Item()

                        try:
                            item.price = pr.find("span", {"class", "s-item__price"})
                            if item.price is not None:
                                item.price = item.price.text.split("$")[1].replace(",", "")
                                if "to" in item.price:
                                    item.price = item.price.replace("to", "")
                                item.price = float(item.price)

                                item.title = pr.find("h3", {"class", "s-item__title"}).text

                                item.url = pr.find("a", {"class", "s-item__link"})["href"]
                                my_list.append(item)

                                dat = "7th step"
                        except Exception as e:
                            print(dat)

                if (len(my_list) > 0):
                    my_list.sort(key=lambda x: x.price, reverse=False)
                    iflistempty = my_list[0].url

                    opener = urllib2.build_opener()
                    opener.addheaders = [('User-agent', 'Mozilla/5.0')]
                    from bs4 import BeautifulSoup
                    soup = BeautifulSoup(opener.open(my_list[0].url).read(), 'html.parser')
                    img = soup.find(attrs={'itemprop': 'image'})
                    if img is not None:
                        img = img['src']

                    my_list[0].image = img
                    temp = soup.find(attrs={'class': 'vi-qtyS-hot-red'})
                    if temp is not None:
                        my_list[0].soldOnpage = temp.find("a").text



                    return my_list[0]
                    break


            except Exception as e:
                return "first"+dat;
        if iflistempty[0] is not 'h':
            iflistempty = "http://" + iflistempty

        opener = urllib2.build_opener()
        opener.addheaders = [('User-agent', 'Mozilla/5.0')]
        from bs4 import BeautifulSoup
        soup = BeautifulSoup(opener.open(iflistempty).read(), 'html.parser')

        highest.image = soup.find(attrs={'itemprop': 'image'})['src']
        highest.soldOnpage = soup.find(attrs={'class': 'vi-qtyS-hot-red'}).find("a").text

        return highest
