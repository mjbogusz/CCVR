#!/usr/bin/env python3

from http.server import SimpleHTTPRequestHandler, HTTPServer
from urllib.parse import parse_qs
import time

class CCVRRequestHandler(SimpleHTTPRequestHandler):
	def do_GET(self):
		# Add 'files' prefix
		self.path = '/files' + self.path
		super().do_GET()

	def do_HEAD(self):
		# Add 'files' prefix
		self.path = '/files' + self.path
		super().do_GET()

	def do_POST(self):
		content_length = int(self.headers['Content-Length'])
		data = parse_qs(self.rfile.read(content_length).decode('utf-8'))
		if not data.get('type') or not data.get('content'):
			self.send_response(400, 'Bad request')
			return

		filename = 'files/'
		if data.get('type')[0] == 'map':
			filename += 'map.txt'
		elif data.get('type')[0] == 'sensors':
			filename += 'sensors.txt'
		else:
			self.send_response(400, 'Bad type')

		try:
			dataFile = open(filename, 'w')
			dataFile.write(data.get('content')[0])
			dataFile.close()

			self.send_response(200, 'OK')
		except Exception as e:
			print('Error writing file:', e)
			self.send_response(500, 'Error writing file')

def run(port = 8080, hostName = ''):
	server_address = (hostName, port)
	server = HTTPServer(server_address, CCVRRequestHandler)
	print(time.asctime(), "Server Starts - %s:%s" % (hostName, port))
	try:
		server.serve_forever()
	except KeyboardInterrupt:
		pass
	server.server_close()
	print(time.asctime(), "Server Stops - %s:%s" % (hostName, port))

if __name__ == "__main__":
	from sys import argv

	if len(argv) == 3:
		run(port = int(argv[1]), hostName = str(argv[2]))
	elif len(argv) == 2:
		run(port = int(argv[1]))
	else:
		run()
