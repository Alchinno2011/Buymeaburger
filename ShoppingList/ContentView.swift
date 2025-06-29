import SwiftUI

struct ShoppingItem: Identifiable, Codable {
    let id: Int
    let userId: Int
    let name: String
    let quantity: String
    var isBought: Bool
    let createdAt: String

    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case userId = "UserId"
        case name = "Name"
        case quantity = "Quantity"
        case isBought = "IsBought"
        case createdAt = "CreatedAt"
    }
}


struct ContentView: View {
    @State private var items: [ShoppingItem] = []
    @State private var showTextField = false
    @State private var showQuantityField = false
    @State private var newItemName = ""
    @State private var Quantity = ""
    @State private var showAlert = false
    @State private var itemToDelete: ShoppingItem? = nil

    var body: some View {
        NavigationView {
            VStack {
                List {
                    ForEach(items.sorted(by: { !$0.isBought && $1.isBought })) { item in
                        HStack {
                            VStack(alignment: .leading) {
                                Text(item.name)
                                    .font(.headline)
                                
                                Text("Quantity: \(item.quantity)")
                                    .font(.subheadline)
                                
                                Text("Added: \(item.createdAt)")
                                    .font(.caption)
                            }
                            Spacer()
                        }
                        .padding()
                        .background(item.isBought ? Color.gray : Color.white)
                        .cornerRadius(8)
                        .contentShape(Rectangle())  // Make whole area tappable
                        .onTapGesture {
                            toggleBought(for: item)
                        }
                        .onLongPressGesture {
                            itemToDelete = item
                            showAlert = true
                        }
                        .alert(item: $itemToDelete) { item in
                            Alert(
                                title: Text("Delete \(item.name)?"),
                                message: Text("Are you sure you want to delete this item?"),
                                primaryButton: .cancel(),
                                secondaryButton: .destructive(Text("Delete")) {
                                    DeleteItem(for: item)
                                }
                            )
                        }
                    }

                }

                // TextField shown when button is pressed
                if showTextField {
                    TextField("Enter item...", text: $newItemName)
                        .textFieldStyle(RoundedBorderTextFieldStyle())
                        .padding()
                        .onSubmit {
                            showTextField = false
                            showQuantityField = true
                        }
                }
                if showQuantityField {
                    TextField("Enter amount...", text: $Quantity)
                        .textFieldStyle(RoundedBorderTextFieldStyle())
                        .padding()
                        .onSubmit {
                            newItem(newItemName, Quantity)
                            showQuantityField = false
                        }
                }

                // + Button
                Button(action: {
                    showTextField.toggle()
                }) {
                    Text("+")
                        .font(.largeTitle)
                        .foregroundColor(.white)
                        .frame(width: 60, height: 60)
                        .background(Color.green)
                        .clipShape(Circle())
                        .shadow(radius: 4)
                }
                .padding()
            }
            .navigationTitle("Shopping List")
            .onAppear(perform: fetchDataFromAPI)
        }
    }
    
    private func DeleteItem(for item: ShoppingItem) {
        print("Delete Item!!! ", item.id)
        
        guard let url = URL(string: "https://simplysite.dk:7001/api/DeleteItem/" + String(item.id)) else { return }
        
        print(url)

        var request = URLRequest(url: url)
        request.httpMethod = "DELETE"  // Set HTTP method to PUT

        // Optional headers
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        // You can add Authorization headers here if needed
        // request.setValue("Bearer YOUR_TOKEN", forHTTPHeaderField: "Authorization")

        let task = URLSession.shared.dataTask(with: request) { data, response, error in
            if let error = error {
                print("Error:", error)
                return
            }

            if let httpResponse = response as? HTTPURLResponse {
                print("Status Code:", httpResponse.statusCode)
                fetchDataFromAPI()
            }

            if let data = data {
                print("Response Body:", String(data: data, encoding: .utf8) ?? "No response body")
            }
        }

        task.resume()
    }
    
    private func newItem(_ name: String, _ Quantity: String) {
        // 1. Define your endpoint URL
        guard let url = URL(string: "https://simplysite.dk:7001/api/newitem") else {
            fatalError("Invalid URL")
        }

        // 2. Define your JSON data
        let json: [String: Any] = [
            "UserId": Int.random(in: 1...100),
            "Name": name,
            "Quantity": Quantity,
            "IsBought": false,
            "CreatedAt": ISO8601DateFormatter().string(from: Date())
        ]

        // 3. Convert JSON to Data
        let jsonData = try? JSONSerialization.data(withJSONObject: json)

        // 4. Create the request
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = jsonData

        // 5. Create and start the data task
        let task = URLSession.shared.dataTask(with: request) { data, response, error in
            if let error = error {
                print("Error: \(error)")
                return
            }

            guard let httpResponse = response as? HTTPURLResponse else {
                print("Invalid response")
                return
            }

            fetchDataFromAPI()
            
            print("Status code: \(httpResponse.statusCode)")

            if let data = data,
               let responseString = String(data: data, encoding: .utf8) {
                print("Response: \(responseString)")
            }
        }
        task.resume()
    }
    
    private func fetchDataFromAPI() {
        guard let url = URL(string: "https://simplysite.dk:7001/api/getGroceries") else { return }

        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("Bearer token", forHTTPHeaderField: "Authorization")
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

        let task = URLSession.shared.dataTask(with: request) { data, response, error in
            if let error = error {
                print("Error:", error)
                return
            }

            guard let httpResponse = response as? HTTPURLResponse,
                  (200...299).contains(httpResponse.statusCode) else {
                print("Server error")
                return
            }

            if let data = data {
                do {
                    let decodedItems = try JSONDecoder().decode([ShoppingItem].self, from: data)
                    DispatchQueue.main.async {
                        self.items = decodedItems
                    }
                } catch {
                    print("JSON decode error:", error)
                }
            }
        }

        task.resume()
    }

    private func toggleBought(for item: ShoppingItem) {
        if let index = items.firstIndex(where: { $0.id == item.id }) {
            items[index].isBought.toggle()
            
            
            
            guard let url = URL(string: "https://simplysite.dk:7001/api/changeState/" + String(item.id)) else { return }
            
            print(url)

            var request = URLRequest(url: url)
            request.httpMethod = "PUT"  // Set HTTP method to PUT

            // Optional headers
            request.setValue("application/json", forHTTPHeaderField: "Content-Type")
            // You can add Authorization headers here if needed
            // request.setValue("Bearer YOUR_TOKEN", forHTTPHeaderField: "Authorization")

            let task = URLSession.shared.dataTask(with: request) { data, response, error in
                if let error = error {
                    print("Error:", error)
                    return
                }

                if let httpResponse = response as? HTTPURLResponse {
                    print("Status Code:", httpResponse.statusCode)
                    fetchDataFromAPI()
                }

                if let data = data {
                    print("Response Body:", String(data: data, encoding: .utf8) ?? "No response body")
                }
            }

            task.resume()
        }
    }
}
