# ProwlNotifier #

This is a little command line tool to send notifications to Prowl (iOS Push Notifiations), see [http://www.prowlapp.com](http://www.prowlapp.com).

Example:

prowlNotifier -a YourApiKey -n ApplicationName -e EventName -d Your message goes here!

The following options are available:

* -a ApiKey from Prowl (required)
* -n Application Name ('Default' if empty)
* -e Event Name ('Default' if empty)
* -d Add current timestamp to the notification message
* -h Show help

Finally the argument is the notification message. If empty "Test" will be used.