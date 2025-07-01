import SwiftUI


import UIKit

class HapticManager {
    static let shared = HapticManager()

    private init() {}

    func impact(style: UIImpactFeedbackGenerator.FeedbackStyle) {
        let generator = UIImpactFeedbackGenerator(style: style)
        generator.prepare()
        generator.impactOccurred()
    }

    func notification(type: UINotificationFeedbackGenerator.FeedbackType) {
        let generator = UINotificationFeedbackGenerator()
        generator.prepare()
        generator.notificationOccurred(type)
    }

    func selection() {
        let generator = UISelectionFeedbackGenerator()
        generator.prepare()
        generator.selectionChanged()
    }
}



extension Color {
    init?(hex: String) {
        var hexSanitized = hex.trimmingCharacters(in: .whitespacesAndNewlines)
        hexSanitized = hexSanitized.hasPrefix("#") ? String(hexSanitized.dropFirst()) : hexSanitized

        var rgb: UInt64 = 0
        guard Scanner(string: hexSanitized).scanHexInt64(&rgb) else {
            return nil
        }

        let length = hexSanitized.count
        if length == 6 {
            let r = Double((rgb & 0xFF0000) >> 16) / 255.0
            let g = Double((rgb & 0x00FF00) >> 8) / 255.0
            let b = Double(rgb & 0x0000FF) / 255.0

            self.init(red: r, green: g, blue: b)
            return
        }

        // You can add support for 8-digit hex (with alpha) if needed
        return nil
    }
    func toHex() -> String? {
        let uiColor = UIColor(self)

        var red: CGFloat = 0
        var green: CGFloat = 0
        var blue: CGFloat = 0
        var alpha: CGFloat = 0

        if uiColor.getRed(&red, green: &green, blue: &blue, alpha: &alpha) {
            let r = Int(red * 255)
            let g = Int(green * 255)
            let b = Int(blue * 255)
            return String(format: "#%02X%02X%02X", r, g, b)
        }

        return nil
    }
}


struct ShoppingItem: Identifiable, Codable {
    let id: Int
    let name: String
    let quantity: String?
    var isBought: Bool
    let colorHex: String?

    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case quantity = "Quantity"
        case isBought = "IsBought"
        case colorHex = "color"
    }

    var color: Color {
        if let hex = colorHex, let color = Color(hex: hex) {
            return color
        }
        return .black
    }
}



struct Recommendation: Identifiable, Codable {
    let id = UUID()
    let name: String
    let colorHex: String

    enum CodingKeys: String, CodingKey {
        case name = "Name"
        case colorHex = "color"
    }
    
    var color: Color {
        Color(hex: colorHex) ?? .black
    }
}




struct ContentView: View {
    @State private var selectedColor: Color = .red

    let availableColors: [Color] = [
        .red, .orange, .yellow, .green,
        .blue, .purple, .pink, .gray
    ]

    
    
    @State private var items: [ShoppingItem] = []
    @State private var showTextField = false
    @State private var showQuantityField = false
    @State private var newItemName = ""
    @State private var Quantity = ""
    @State private var showAlert = false
    @State private var itemToDelete: ShoppingItem? = nil
    @FocusState private var isTextFieldFocused: Bool
    @State private var recommendations: [Recommendation] = []


    var body: some View {
        NavigationView {
            ZStack {
                VStack {
                    List {
                        ForEach(items.sorted(by: { !$0.isBought && $1.isBought })) { item in
                            HStack {
                                VStack(alignment: .leading) {
                                    Text(item.name).strikethrough(item.isBought)
                                    Text(item.quantity ?? "")
                                }
                                Spacer()
                            }
                            .padding()
                            .background(item.isBought ? Color.black.opacity(0.2) : Color.white)
                            .cornerRadius(8)
                            .overlay(RoundedRectangle(cornerRadius: 8).stroke(item.color, lineWidth: 2))
                            .contentShape(Rectangle())
                            .onTapGesture
                            {
                                HapticManager.shared.notification(type: .success)
                                toggleBought(for: item)
                            }
                            .onLongPressGesture {
                                itemToDelete = item
                                showAlert = true
                            }
                        }
                    }
                    .alert(item: $itemToDelete) { item in
                        Alert(
                            title: Text("Delete \(item.name)?"),
                            primaryButton: .cancel(),
                            secondaryButton: .destructive(Text("Delete")) {
                                DeleteItem(for: item)
                            }
                        )
                    }
                    .blur(radius: showTextField ? 5 : 0)
                    .allowsHitTesting(!showTextField)
                    
                    
                    Button(action: {
                        showTextField = true
                        isTextFieldFocused = true
                        newItemName = ""
                        HapticManager.shared.impact(style: .medium)
                        GetRecomendations()
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
                

                VStack(spacing: 10) {
                    if showTextField {
                        HStack {
                            TextField("Enter item...", text: $newItemName)
                                .textFieldStyle(RoundedBorderTextFieldStyle())
                                .padding(8)
                                .background(Color(.systemBackground))
                                .cornerRadius(10)
                                .focused($isTextFieldFocused)
                                .onSubmit {
                                    showTextField = false
                                    isTextFieldFocused = false
                                    HapticManager.shared.notification(type: .success)
                                    newItem(newItemName, selectedColor)
                                }
                                .onChange(of: newItemName) {
                                    HapticManager.shared.impact(style: .light)
                                }

                            Button(action: {
                                showTextField = false
                                isTextFieldFocused = false
                                HapticManager.shared.notification(type: .warning)
                            }) {
                                Text("Cancel")
                                    .padding(.horizontal, 16)
                                    .padding(.vertical, 10)
                                    .background(Capsule().fill(Color.red))
                                    .foregroundColor(.white)
                            }
                        }
                        .padding(.horizontal)

                        HStack(spacing: 15) {
                            ForEach(availableColors, id: \.self) { color in
                                Circle()
                                    .fill(color)
                                    .frame(width: 30, height: 30)
                                    .overlay(
                                        Circle()
                                            .stroke(selectedColor == color ? Color.black : Color.clear, lineWidth: 2)
                                    )
                                    .onTapGesture {
                                        selectedColor = color
                                    }
                                
                                    .onChange(of: selectedColor) {
                                        HapticManager.shared.selectionChanged()
                                    }
                            }
                        }
                        .padding(.horizontal)

                        VStack {
                            List {
                                ForEach(recommendations) { recommendation in
                                    HStack {
                                        VStack(alignment: .leading) {
                                            Text(recommendation.name) // fixed typo here
                                                .font(.headline)
                                        }
                                        Spacer()
                                    }
                                    .padding()
                                    .background(Color.white)
                                    .cornerRadius(8)
                                    .overlay(
                                        RoundedRectangle(cornerRadius: 8)
                                            .stroke(recommendation.color, lineWidth: 2)
                                    )
                                    .contentShape(Rectangle())
                                    .onTapGesture {
                                        showTextField = false
                                        isTextFieldFocused = false
                                        HapticManager.shared.notification(type: .success)
                                        newItem(recommendation.name, recommendation.color)
                                    }
                                }
                            }
                        }
                    }
                    Spacer()
                }
            }
            .navigationTitle("Shopping List")
            .onAppear(perform: fetchDataFromAPI)
            .refreshable {
                fetchDataFromAPI()
            }
        }
    }

        
        private func GetRecomendations() {
            guard let url = URL(string: "https://simplysite.dk:7001/api/GetRecomendations") else { return }

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
                        let decodedItems = try JSONDecoder().decode([Recommendation].self, from: data)
                        DispatchQueue.main.async {
                            self.recommendations = decodedItems
                        }
                    } catch {
                        print("JSON decode error:", error)
                    }
                }
            }

            task.resume()
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
    
    private func newItem(_ input: String, _ color: Color) {
        guard let url = URL(string: "https://simplysite.dk:7001/api/newitem") else {
            fatalError("Invalid URL")
        }

        let parts = input.split(separator: ":", maxSplits: 1).map { $0.trimmingCharacters(in: .whitespaces) }

        var json: [String: Any] = [
            "UserId": Int.random(in: 1...100),
            "IsBought": false,
            "CreatedAt": ISO8601DateFormatter().string(from: Date()),
            "color": color.toHex()
        ]

        if parts.count == 2 {
            json["Name"] = parts[0]
            json["Quantity"] = parts[1]
            print("Quantity: \(parts[1])")
        } else {
            json["Name"] = parts[0]
            print("No quantity specified")
        }

        guard let jsonData = try? JSONSerialization.data(withJSONObject: json) else {
            print("Failed to serialize JSON")
            return
        }

        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.httpBody = jsonData

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
        guard let index = items.firstIndex(where: { $0.id == item.id }) else { return }

        withAnimation {
            items[index].isBought.toggle()
        }

        guard let url = URL(string: "https://simplysite.dk:7001/api/changeState/" + String(item.id)) else { return }

        var request = URLRequest(url: url)
        request.httpMethod = "PUT"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

        let task = URLSession.shared.dataTask(with: request) { data, response, error in
            if let error = error {
                print("Error:", error)
                return
            }
            if let httpResponse = response as? HTTPURLResponse {
                print("Status Code:", httpResponse.statusCode)
                // Consider commenting out this call during animation, or debounce to avoid UI flicker:
                fetchDataFromAPI()
            }
            if let data = data {
                print("Response Body:", String(data: data, encoding: .utf8) ?? "No response body")
            }
        }
        task.resume()
    }
}
