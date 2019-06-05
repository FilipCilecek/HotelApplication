# Hotel_Application
Aplikace sloužící pro funkci recepce. Tzn. rezervace hostů, přidávání pokojů, vlastnosti pokojů, pokoje propojeny s vybavením, které je spojeno se skladem, správa skladu hotelu, placení a další. 

MainWindow - hlavní okno v kterém se bude dále zobrazovat content (další podokna - <UserControl>)

AdminControlWindow - okno admina (tentokrát bez loginu - samozřejmě by se dal dodělat) - zde se nachází :
 - vytvoření/odebrání produktu, vytvoření/odebrání skladu, vytvoření/odebrání lednice (každý pokoj má lednici), přidání/odebrání pokoje,
 výměna lednice v pokoji, přidaní/odebrání vlastnosti (pokoj může mít své originální vlastnosti)

HelpControlWindow - poslání e-mailu na podporu

PayControlWindow - vybrání rezervace, podle toho se zpočítá počet pokojů, k ceně se může připočítat, pokud si klient něco koupil z lednice, následně vypočítá ceny všech pokojů za všechny dny a uloží do databáze a vytvoří JSON file. (nedodělán akorát výpis na obrazovce všech položek do groupboxu)

ReservationControlWindow - vytvoření rezervace. Zautomatizovaný výpis pokojů, které jsou volné na dané rozmezí datumů, pro rychlejší přehled. Dále také výběr pokoje a následný výpis všech rezervací pro tento pokoj.

StorageControlWindow - práce se skladem - vytvoření produktu, přidání produktu do skladu, následné přesunutí produktů ze skladu do lednice. Dále přehled produktů ve vybraném skladu/lednici.
